
using Aspor.Authorization.Schema.Conditions;
using Aspor.Authorization.User;
using System.Collections.Generic;

namespace Aspor.Authorization.Schema.Element
{
    public class ConditionPermissionElement : IPermissionElement
    {

        private readonly IPermissionCondition _condition;
        private readonly IList<IPermissionElement> _children;

        public ConditionPermissionElement(IPermissionCondition condition, IList<IPermissionElement> children)
        {
            _condition = condition;
            _children = children;
        }

        public void AppendPermissions(AsporUser user, IList<string> permissions)
        {
            if (_condition.Matches(user))
            {
                foreach (IPermissionElement node in _children)
                {
                    node.AppendPermissions(user, permissions);
                }
            }
        }
    }
}
