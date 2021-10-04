namespace Aspor.Authorization.Schema.Conditions
{
    public interface IPermissionConditionFactory
    {

        public IPermissionCondition Create(string[] parameters);

    }
}
