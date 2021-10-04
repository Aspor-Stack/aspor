using Aspor.Authorization.User;

namespace Aspor.Authorization.Schema.Conditions
{

    public class GroupPermissionCondition : IPermissionCondition
    {

        private readonly string _group;

        public GroupPermissionCondition(string group)
        {
            _group = group;
        }

        public bool Matches(AsporUser user)
        {
            return user.Groups.Contains(_group);
        }

        public class Factory : IPermissionConditionFactory
        {
            public IPermissionCondition Create(string[] parameters)
            {
                return new GroupPermissionCondition(parameters[0]);
            }
        }
    }
}
