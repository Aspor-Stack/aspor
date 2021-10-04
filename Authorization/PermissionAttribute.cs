using System;

namespace Aspor.Authorization
{
    public class PermissionAttribute : Attribute
    {

        public string Permission { get; set; }

        public bool DisallowRowPermissions { get; set; } = false;
    }
}
