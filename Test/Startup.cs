using Aspor.Common.Extensions;
using Aspor.Streaming;
using Aspor.Streaming.Core.Extensions;
using Aspor.Validation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Streaming;
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
            services.AddCors();
            services.AddAuthorization();
            services.AddRouting();
            services.AddODataBatchHandler();

            services.AddAsporValidation();
            services.AddAsporStreaming((provider=>
            {
                provider.UseMemoryBus();
            }));

            services.AddControllers()
                  .AddAutoPreValidationCheck()
                  .AddAsporODataStreaming(StreamMode.AUTO)
                  .AddAsporODataPageSize()
                  .AddAsporReturnPreference()
                  .AddAsporETagAutoMatch()
                  .AddOData(options =>
                  {
                      options.EnableQueryFeatures(1000);
                      options.AddRouteComponents("api", EdmModelConfiguration.Configure());
                  });

            services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("test"));
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
