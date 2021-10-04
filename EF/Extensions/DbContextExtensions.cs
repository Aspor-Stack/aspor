using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Aspor.EF.Extensions
{
    public static class DbContextExtensions
    {

        public static EntityEntry RemoveFull(this DbContext context, [NotNull] object entity)
        {
            if (entity is IEntityTimestamps entityTimes)
            {
                entityTimes.DeletedOn = DateTime.UtcNow;
            }
            return context.Remove(entity);
        }

        public static EntityEntry<TEntity> RemoveFull<TEntity>(this DbContext context, [NotNull] TEntity entity) where TEntity : class
        {
            if (entity is IEntityTimestamps entityTimes)
            {
                entityTimes.DeletedOn = DateTime.UtcNow;
            }
            return context.Remove<TEntity>(entity);
        }

        public static void RemoveRangeFull(this DbContext context, [NotNull] params object[] entities)
        {
            foreach (object entity in entities)
            {
                if (entity is IEntityTimestamps entityTimes)
                {
                    entityTimes.DeletedOn = DateTime.UtcNow;
                }
            }
            context.RemoveRange(entities);
        }

        public static void RemoveRangeFull(this DbContext context, [NotNull] IEnumerable<object> entities)
        {
            foreach (object entity in entities)
            {
                if (entity is IEntityTimestamps entityTimes)
                {
                    entityTimes.DeletedOn = DateTime.UtcNow;
                }
            }
            context.RemoveRange(entities);
        }

    }
}
