
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;

namespace Aspor.Common.Extensions
{
    public static class EdmModelExtension
    {

        private static readonly IDictionary<IEdmModel, IDictionary<IEdmStructuredType, string[]>> _virtualProperties = new Dictionary<IEdmModel, IDictionary<IEdmStructuredType, string[]>>();

        public static IDictionary<IEdmStructuredType, string[]> GetVirtualNavigations(this IEdmModel model)
        {
            if (_virtualProperties.TryGetValue(model, out var result)) return result;
            else return null;
        }

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

            IDictionary<IEdmStructuredType, string[]> dictonary;
            if (!_virtualProperties.TryGetValue(model, out dictonary))
            {
                dictonary = new Dictionary<IEdmStructuredType, string[]>();
                _virtualProperties[model] = dictonary;
            }

            string[] properties;
            if (!dictonary.TryGetValue(sourceType, out properties))
            {
                properties = new string[0];
            }
            Array.Resize(ref properties, properties.Length + 1);
            properties[properties.Length - 1] = name;
            dictonary[sourceType] = properties;

            return model;
        }

    }
}
