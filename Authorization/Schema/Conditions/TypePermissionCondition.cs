using Aspor.Authorization.User;
using System;

namespace Aspor.Authorization.Schema.Conditions
{
    public class TypePermissionCondition : IPermissionCondition
    {

        private readonly UserType _type;

        public TypePermissionCondition(UserType type)
        {
            _type = type;
        }

        public bool Matches(AsporUser user)
        {
            return user.Type == _type;
        }

        public class Factory : IPermissionConditionFactory
        {
            public IPermissionCondition Create(string[] parameters)
            {
                return new TypePermissionCondition((UserType)Enum.Parse(typeof(UserType), parameters[0].ToUpper()));
            }
        }
    }
}
