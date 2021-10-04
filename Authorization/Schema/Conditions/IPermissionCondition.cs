using Aspor.Authorization.User;

namespace Aspor.Authorization.Schema.Conditions
{
    public interface IPermissionCondition
    {

        public bool Matches(AsporUser user);

    }
}
