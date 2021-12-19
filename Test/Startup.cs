using Aspor.Authorization;
using Aspor.Authorization.Extensions;
using Aspor.Common;
using Aspor.Common.Extensions;
using Aspor.Export;
using Aspor.Export.Extensions;
using Aspor.Streaming;
using Aspor.Streaming.Core.Extensions;
using Aspor.Validation.Extensions;
using Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Test.Api;
using Test.Model;

namespace Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            QueryPolicyContext context = new QueryPolicyContext();
            context.create<Board>(user => board => board.Name == "Suggestions");
            context.create<Project>(user => board => false);

            services.AddCors();
            services.AddAuthorization();
            services.AddRouting();
            services.AddODataBatchHandler();
            services.AddSingleton(context);

            services.AddAsporValidation();
            services.AddAsporStreaming((provider =>
            {
                provider.UseMemoryBus();
            }));

            services.AddControllers()
                  .AddMvcOptions((options) => {
                      options.Filters.Add(new AsporAuthorizationFilter());
                  })
                  .AddAutoPreValidationCheck()
                  .AddAsporODataStreaming(StreamMode.AUTO)
                  .AddAsporODataPageSize()
                  .AddAsporReturnPreference()
                  .AddAsporETagAutoMatch()
                  .AddAsporExport()
                  .AddOData(options =>
                  {
                      options.EnableQueryFeatures(1000);
                      options.AddRouteComponents("api", EdmModelConfiguration.Configure(), (component) =>
                      {
                          component.AddVirtualSelectExpandValidator();
                          //component.AddPolicyBinder(context);
                      });
                  });

            string connection = Env.Get("DATABASE");
            if (connection.Equals("local")) services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("project-front"));
            services.AddDbContext<DatabaseContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            //app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseODataRouteDebug();
            app.UseODataBatching();
            app.UseODataQueryRequest();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
