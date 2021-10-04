using System.Collections.Generic;

namespace Aspor.Authorization.Schema.Conditions
{
    public class PermissionConditionRegistry
    {

        private static readonly IDictionary<string, IPermissionConditionFactory> Factories = new Dictionary<string, IPermissionConditionFactory>();

        static PermissionConditionRegistry()
        {
            Register("all", new AllPermissionCondition.Factory());
            Register("type", new TypePermissionCondition.Factory());
            Register("scope", new ScopePermissionCondition.Factory());
            Register("group", new GroupPermissionCondition.Factory());
        }

        public static void Register(string type, IPermissionConditionFactory factory)
        {
            Factories[type] = factory;
        }

        public static IPermissionCondition Parse(string value)
        {
            int typeIndex = value.IndexOf(":");
            string[] parameters;
            string type;

            if (typeIndex > 0)
            {
                type = value.Substring(0, typeIndex);
                parameters = value.Substring(typeIndex + 1).Split(";");
            }
            else
            {
                type = value;
                parameters = new string[0];
            }

            IPermissionConditionFactory factory = Factories[type];
            return factory.Create(parameters);
        }

    }
}
