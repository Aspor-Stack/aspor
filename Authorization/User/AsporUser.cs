using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Aspor.Authorization.User
{
    public class AsporUser
    {

        public bool IsAuthenticated { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public UserType Type { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public IList<string> Scopes { get; set; }

        public IList<string> Groups { get; set; }
    }
}
