using Aspor.Authorization.User;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Authorization.Policy
{
    public class QueryPolicyContext
    {

        private readonly IDictionary<Type, Func<AsporUser, Expression>> _conditions = new Dictionary<Type, Func<AsporUser, Expression>>();

        public void create<T>(Func<AsporUser, Expression<Func<T, bool>>> condition)
        {
            _conditions[typeof(T)] = condition;
        }

        public Func<AsporUser, Expression> GetConditionExpressionFunction(Type type)
        {
            if (_conditions.TryGetValue(type, out var condition)) return condition;
            return null;
        }

    }
}
