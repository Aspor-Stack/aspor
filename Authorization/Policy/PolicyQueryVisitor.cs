using Aspor.Authorization.Extensions;
using Aspor.Authorization.User;
using Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNetCore.OData
{
    public class PolicyQueryVisitor : ExpressionVisitor
    {

        private static readonly PropertyInfo PROPERTY =  typeof(ODataQueryContext).GetProperty("Request", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private readonly QueryPolicyContext _policy;
        // private readonly SelectExpandBinderContext _context;

        /*
         * public PolicyQueryVisitor(QueryPolicyContext policy, SelectExpandBinderContext context)
        {
            _policy = policy;
            _context = context;
        }
         * 
         */

        public AsporUser GetUser()
        {
            return null;
         //    return ((HttpRequest)PROPERTY.GetValue(_context.SelectExpand.Context)).HttpContext.GetUser();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(IOrderedEnumerable<>))
            {
                var type = node.Method.ReturnType.GetGenericArguments()[0];
                Func<AsporUser, Expression> condition = _policy.GetConditionExpressionFunction(type);
                if(condition != null)
                {
                    var whereExpression = Expression.Call(
                        typeof(Enumerable),
                        nameof(Enumerable.Where),
                        node.Method.ReturnType.GetGenericArguments(),
                        node.Arguments[0], 
                        condition.Invoke(GetUser()));

                    return Expression.Call(node.Method, whereExpression, node.Arguments[1]);
                }
            }
            return base.VisitMethodCall(node);
        }

    }
}
