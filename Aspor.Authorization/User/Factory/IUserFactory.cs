using Microsoft.AspNetCore.Http;

namespace Aspor.Authorization.User.Factory
{
    public interface IUserFactory
    {

        public AsporUser Create(HttpContext context);

    }
}
