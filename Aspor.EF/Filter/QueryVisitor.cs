using Aspor.EF;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.OData
{
    public class QueryVisitor : ExpressionVisitor
    {

        public static QueryVisitor INSTANCE = new QueryVisitor();

        private static readonly Expression FILTER_EXPRESSION = CreateExpression<IEntityTimestamps>(entity => entity.DeletedOn == null);

        private QueryVisitor() {}

        private static Expression CreateExpression<T>(Expression<Func<T, bool>> builder) where T : class, IEntityTimestamps
        {
            return builder;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(IOrderedEnumerable<>))
            {
                var type = node.Method.ReturnType.GetGenericArguments()[0];
                if(typeof(IEntityTimestamps).IsAssignableFrom(type))
                {
                    var parameter = Expression.Parameter(type, "entity");

                    var member = Expression.MakeMemberAccess(parameter, type.GetProperty("DeletedOn"));

                    var body = Expression.Equal(member, Expression.Constant(null));

                    var delegateType = typeof(Func<,>).MakeGenericType(type, typeof(bool));

                    var filterExpression = Expression.Lambda(delegateType, body, parameter);

                    var whereExpression = Expression.Call(
                      typeof(Enumerable),
                      nameof(Enumerable.Where),
                      node.Method.ReturnType.GetGenericArguments(),
                      node.Arguments[0], filterExpression);

                    return Expression.Call(node.Method, whereExpression, node.Arguments[1]);
                }
            }
            return base.VisitMethodCall(node);
        }

    }
}
