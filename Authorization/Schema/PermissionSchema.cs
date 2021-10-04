using Aspor.Authorization.Schema.Element;
using Aspor.Authorization.Schema.Node;
using Aspor.Authorization.User;
using System.Collections.Generic;

namespace Aspor.Authorization.Schema
{
    public class PermissionSchema : IPermissionElement
    {

        private readonly IList<IPermissionElement> _children;

        public PermissionSchema(IList<IPermissionElement> children)
        {
            _children = children;
        }

        public void AppendPermissions(AsporUser user, IList<string> permissions)
        {
            foreach (IPermissionElement node in _children)
            {
                node.AppendPermissions(user, permissions);
            }
        }

        public bool HasPermission(AsporUser user, IPermissionNode node)
        {
            return true;
        }

    }
}
