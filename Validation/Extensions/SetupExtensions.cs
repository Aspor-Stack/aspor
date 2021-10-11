using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Aspor.Validation.Extensions
{

    public static class SetupExtensions
    {

        public static IServiceCollection AddAsporValidation(this IServiceCollection services)
        {
            return services.AddSingleton<IObjectModelValidator, AsporObjectModelValidator>();
        }

        public static IMvcBuilder AddAutoPreValidationCheck(this IMvcBuilder mvc)
        {
            mvc.AddMvcOptions(config =>
            {
                config.Filters.Add(new AsporValidationFilter());
            });
            return mvc;
        }

    }

}
