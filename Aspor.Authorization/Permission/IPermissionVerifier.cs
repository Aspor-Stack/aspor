using Aspor.Authorization.User;
using System.Threading.Tasks;

namespace Authorization.Permission
{
    public interface IPermissionVerifier
    {

        public Task<bool> HasPermission(AsporUser user, string permission, object key);

    }
}
