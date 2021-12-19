using Aspor.Authorization.User;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Authorization.Policy
{
    public class PolicySelectExpandBinder : SelectExpandBinder
    {

        private readonly QueryPolicyContext _policy;

        public PolicySelectExpandBinder(QueryPolicyContext policy)
        {
            _policy = policy;
        }

        public override IQueryable Bind(IQueryable source, SelectExpandBinderContext context)
        {
            var visitor = new PolicyQueryVisitor(_policy, context);

            Func<AsporUser, Expression> condition = _policy.GetConditionExpressionFunction(source.ElementType);
            if(condition != null) {
                var baseExpression = Expression.Call(
                    typeof(Queryable), 
                    nameof(Queryable.Where), 
                    new[] { source.ElementType },
                    source.Expression, condition.Invoke(visitor.GetUser()));
                source = source.Provider.CreateQuery(baseExpression);
            }

            IQueryable queryable = base.Bind(source, context);
            var newExpression = visitor.Visit(queryable.Expression);
            return queryable.Provider.CreateQuery(newExpression);
        }
    }
}
