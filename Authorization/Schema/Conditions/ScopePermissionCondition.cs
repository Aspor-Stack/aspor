using Aspor.Authorization.User;

namespace Aspor.Authorization.Schema.Conditions
{

    public class ScopePermissionCondition : IPermissionCondition
    {

        public static string Prefix = null;

        private readonly string _scope;

        public ScopePermissionCondition(string scope)
        {
            _scope = scope;
        }

        public bool Matches(AsporUser user)
        {
            return user.Scopes.Contains(_scope);
        }

        public class Factory : IPermissionConditionFactory
        {
            public IPermissionCondition Create(string[] parameters)
            {
                return new ScopePermissionCondition(Prefix != null ? Prefix + parameters[0] : parameters[0]);
            }
        }
    }
}
