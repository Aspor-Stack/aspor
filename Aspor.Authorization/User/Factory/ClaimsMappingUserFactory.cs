using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;

namespace Aspor.Authorization.User.Factory
{
    public class ClaimsMappingUserFactory : IUserFactory
    {

        private readonly IDictionary<PropertyInfo, Func<ClaimsPrincipal, object>> _mappers;

        public ClaimsMappingUserFactory(IDictionary<PropertyInfo, Func<ClaimsPrincipal, object>> mappers)
        {
            _mappers = mappers;
        }

        public AsporUser Create(HttpContext context)
        {
            AsporUser user = new AsporUser();
            user.Groups = new List<string>();
            user.Scopes = new List<string>();
            foreach (KeyValuePair<PropertyInfo, Func<ClaimsPrincipal, object>> entry in _mappers)
            {
                object result = null;
                try
                {
                    result = entry.Value.Invoke(context.User);
                }
                catch (Exception) { }

                if (result == null)
                {
                    return null;
                }

                entry.Key.SetValue(user, result);
            }
            return user;
        }
    }
}
