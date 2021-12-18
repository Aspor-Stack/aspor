
using Microsoft.OData.Edm;

namespace Common.Extensions
{
    public static class EdmModelExtension
    {

        public static IEdmModel AddVirtualNavigation(this IEdmModel model, string sourceSet, string targetSet, string name) 
        {
            var source = (EdmEntitySet)model.FindDeclaredEntitySet(sourceSet);
            var target = (EdmEntitySet)model.FindDeclaredEntitySet(targetSet);
            var sourceType = (EdmEntityType)source.Type.AsElementType();
            var targetType = (EdmEntityType)target.Type.AsElementType();

            var property = new EdmNavigationPropertyInfo();
            property.TargetMultiplicity = EdmMultiplicity.Many;
            property.Target = targetType;
            property.ContainsTarget = false;
            property.OnDelete = EdmOnDeleteAction.None;
            property.Name = name;

            source.AddNavigationTarget(sourceType.AddUnidirectionalNavigation(property), target);

            return model;
        }

    }
}
