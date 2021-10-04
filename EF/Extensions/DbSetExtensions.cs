using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Aspor.EF.Extensions
{
    public static class DbSetExtensions
    {

        public static EntityEntry<TEntity> RemoveFull<TEntity>(this DbSet<TEntity> set, TEntity entity) where TEntity : class
        {
            if (entity is IEntityTimestamps entityTimes)
            {
                entityTimes.DeletedOn = DateTime.UtcNow;
            }
            return set.Remove(entity);
        }

        public static void RemoveRangeFull<TEntity>(this DbSet<TEntity> set, [NotNull] params TEntity[] entities) where TEntity : class
        {
            foreach (TEntity entity in entities)
            {
                if (entity is IEntityTimestamps entityTimes)
                {
                    entityTimes.DeletedOn = DateTime.UtcNow;
                }
            }
            set.RemoveRange(entities);
        }

        public static void RemoveRangeFull<TEntity>(this DbSet<TEntity> set, [NotNull] IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (TEntity entity in entities)
            {
                if (entity is IEntityTimestamps entityTimes)
                {
                    entityTimes.DeletedOn = DateTime.UtcNow;
                }
            }
            set.RemoveRange(entities);
        }

        public static IQueryable<TEntity> Active<TEntity>(this DbSet<TEntity> set) where TEntity : class, IEntityTimestamps
        {
            return set.Where(e => e.DeletedOn == null);
        }

        public static IQueryable<TEntity> Deleted<TEntity>(this DbSet<TEntity> set) where TEntity : class, IEntityTimestamps
        {
            return set.Where(e => e.DeletedOn != null);
        }

        public static IQueryable<TEntity> Unchanged<TEntity>(this DbSet<TEntity> set) where TEntity : class, IEntityTimestamps
        {
            return set.Where(e => e.DeletedOn == null && e.CreatedOn == e.ModifiedOn);
        }

        public static IQueryable<TEntity> Changed<TEntity>(this DbSet<TEntity> set) where TEntity : class, IEntityTimestamps
        {
            return set.Where(e => e.DeletedOn == null && e.ModifiedOn != e.CreatedOn);
        }
    }
}
