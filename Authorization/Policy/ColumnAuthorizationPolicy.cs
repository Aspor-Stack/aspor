using System.Linq;

namespace Aspor.Authorization.Policy
{
    public interface ColumnAuthorizationPolicy
    {

        public void ApplyPolicy(IQueryable queryable);

    }
}
