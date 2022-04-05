using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

namespace Aspor.Authorization.User.Factory
{
    public class ClaimsMappingBuilder
    {

        private readonly IDictionary<PropertyInfo, Func<ClaimsPrincipal, object>> _mappers = new Dictionary<PropertyInfo, Func<ClaimsPrincipal, object>>();

        public ClaimsMappingBuilder Map(Expression<Func<AsporUser, object>> expression, string claim)
        {
            PropertyInfo info = (PropertyInfo)((MemberExpression)((UnaryExpression)expression.Body).Operand).Member;

            if (info.PropertyType is IList<object>)
            {

            }
            //_dictionary[info] = result;

            return this;
        }

        public ClaimsMappingBuilder Map<T>(Expression<Func<AsporUser, T>> expression, Func<ClaimsPrincipal, T> result)
        {
            PropertyInfo info = (PropertyInfo)((MemberExpression)expression.Body).Member;
            _mappers[info] = result as Func<ClaimsPrincipal, object>;
            return this;
        }

        public ClaimsMappingUserFactory Create()
        {
            return new ClaimsMappingUserFactory(_mappers);
        }

        public static ClaimsMappingUserFactory Test()
        {
            ClaimsMappingBuilder test = new ClaimsMappingBuilder();
            //  test.Map((u) => u.Id,"itd");
            test.Map((u) => u.DisplayName, (c) => c.Claims.FirstOrDefault(c => c.Type == "name")?.Value);
            test.Map((u) => u.Username, (c) => c.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);
            return test.Create();
        }

    }
}
