using Aspor.Common.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Test.Model;

namespace Test.Api
{
    public class EdmModelConfiguration
    {

        public static IEdmModel Configure()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();

            builder.EntitySet<Project>("projects").EntityType.Configure((user) =>
            {

            });

            return builder.GetEdmModel();
        }

    }
}
