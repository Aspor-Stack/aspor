using System;
using System.Collections.Generic;

namespace Aspor.Validation.Definition
{
    public class ValidationDefinitionCache
    {
        private static readonly IDictionary<Type, ValidationDefinition> Definitions = new Dictionary<Type, ValidationDefinition>();

        public static ValidationDefinition Get(Type type)
        {
            ValidationDefinition definition;
            if (!Definitions.TryGetValue(type, out definition))
            {
                definition = ValidationDefinitionBuilder.Build(type);
                Definitions.Add(type, definition);
            }
            return definition;
        }

        public static void Reset()
        {
            Definitions.Clear();
        }
    }
}
