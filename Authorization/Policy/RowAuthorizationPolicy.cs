using System.Linq;

namespace Aspor.Authorization.Policy
{
    public interface RowAuthorizationPolicy
    {

        public void ApplyPolicy(IQueryable queryable);

        public bool ValidatePolicy(object entity);

    }
}
