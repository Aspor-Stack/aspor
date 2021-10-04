using Aspor.Authorization.User;

namespace Aspor.Authorization.Schema.Conditions
{
    public class AllPermissionCondition : IPermissionCondition
    {


        public bool Matches(AsporUser user)
        {
            return true;
        }

        public class Factory : IPermissionConditionFactory
        {
            public IPermissionCondition Create(string[] parameters)
            {
                return new AllPermissionCondition();
            }
        }
    }
}
