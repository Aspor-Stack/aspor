using System;

namespace Aspor.EF
{
    public interface IEntityTenancy
    {
        Guid TenantId { get; set; }
    }
}
