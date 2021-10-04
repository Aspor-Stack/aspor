
namespace Aspor.Authorization.Schema.Node
{
    public interface IPermissionNode
    {

        IPermissionNode Next { get; }

        bool MatchesValue(object value);

        bool Matches(IPermissionNode compare);

    }
}
