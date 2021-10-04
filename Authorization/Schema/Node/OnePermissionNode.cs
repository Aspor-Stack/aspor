/*
 *  test.user | test.user
 *  test.user | test
 *  test | test.user
 * 
 * */
namespace Aspor.Authorization.Schema.Node
{
    public class OnePermissionNode : IPermissionNode
    {

        private object _value;
        private IPermissionNode _next;

        public OnePermissionNode(object value, IPermissionNode next)
        {
            _value = value;
            _next = next;
        }

        public IPermissionNode Next => _next;

        public bool MatchesValue(object value)
        {
            return value.Equals(_value);
        }

        public bool Matches(IPermissionNode compare)
        {
            if (compare.MatchesValue(_value))
            {
                if (Next != null) return compare.Next != null && Next.Matches(compare.Next);
                else return compare.Next == null;
            }
            return false;
        }
    }
}
