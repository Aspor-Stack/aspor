namespace Aspor.Authorization.Schema.Node
{
    public class PermissionNodeParser
    {

        public static IPermissionNode Parse(string permission)
        {
            string[] parts = permission.Split(".");
            foreach (string part in parts)
            {
                // if(part.Equals("*")) 
            }
            return null;
        }

        private static IPermissionNode ParseValue(string value)
        {
            return null;
        }
    }
}
