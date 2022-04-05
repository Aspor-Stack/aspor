using System;

namespace Aspor.EF
{
    public interface IEntityExecutors
    {
        Guid CreatedBy { get; set; }

        Guid ModifiedBy { get; set; }

        Guid? DeletedBy { get; set; }
    }
}
