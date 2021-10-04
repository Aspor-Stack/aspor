/*
 *  test.user | test.user
 *  test.user | test
 *  test | test.user
 * 
 * */
namespace Aspor.Authorization.Schema.Node
{
    public class AlwaysPermissionNode : IPermissionNode
    {

        private IPermissionNode _next;

        public AlwaysPermissionNode(IPermissionNode next)
        {
            _next = next;
        }

        public IPermissionNode Next => _next;

        public bool MatchesValue(object compare)
        {
            return true;
        }

        public bool Matches(IPermissionNode compare)
        {
            if (Next != null) return compare.Next != null && Next.Matches(compare.Next);
            else return true;
        }
    }
}
