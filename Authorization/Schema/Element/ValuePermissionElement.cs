using Aspor.Authorization.User;
using System.Collections.Generic;

namespace Aspor.Authorization.Schema.Element
{
    public class ValuePermissionElement : IPermissionElement
    {

        private readonly string _permission;

        public ValuePermissionElement(string permission)
        {
            _permission = permission;
        }

        public void AppendPermissions(AsporUser user, IList<string> permissions)
        {
            permissions.Add(_permission);
        }
    }
}
