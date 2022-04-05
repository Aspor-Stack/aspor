using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Aspor.EF
{
    public interface IEntityTimestamps
    {
        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }

        DateTime? DeletedOn { get; set; }

        public static void UpdateTimestamps(object sender, EntityEntryEventArgs e)
        {
            if (e.Entry.Entity is IEntityTimestamps entity)
            {
                switch (e.Entry.State)
                {
                    case EntityState.Deleted:
                        if (entity.DeletedOn == null)
                        {
                            entity.DeletedOn = DateTime.Now;
                            e.Entry.State = EntityState.Modified;
                        }
                        break;
                    case EntityState.Modified:
                        if (entity.DeletedOn != null)
                        {
                            entity.ModifiedOn = DateTime.Now;
                        }
                        break;
                    case EntityState.Added:
                        entity.CreatedOn = DateTime.Now;
                        entity.ModifiedOn = entity.CreatedOn;
                        break;
                }
            }
        }
    }
}
