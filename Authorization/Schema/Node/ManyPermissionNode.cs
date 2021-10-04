/*
 *  test.user | test.user
 *  test.user | test
 *  test | test.user
 * 
 * */
namespace Aspor.Authorization.Schema.Node
{
    public class ManyPermissionNode : IPermissionNode
    {

        private object[] _values;
        private IPermissionNode _next;

        public ManyPermissionNode(object[] values, IPermissionNode next)
        {
            _values = values;
            _next = next;
        }

        public IPermissionNode Next => _next;

        public bool MatchesValue(object compare)
        {
            foreach (object value in _values)
            {
                return value.Equals(compare);
            }
            return false;
        }

        public bool Matches(IPermissionNode compare)
        {
            bool result = false;
            foreach (object value in _values)
            {
                if (compare.MatchesValue(value))
                {
                    result = true;
                    break;
                }
            }

            if (result)
            {
                if (Next != null) return compare.Next != null && Next.Matches(compare.Next);
                else return compare.Next == null;
            }

            return result;
        }
    }
}
