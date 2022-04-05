using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aspor.EF.Extensions
{
    public static class ChangeTrackerExtensions
    {

        public static void AddAutoTimestampUpdate(this ChangeTracker tracker)
        {
            tracker.StateChanged += IEntityTimestamps.UpdateTimestamps;
            tracker.Tracked += IEntityTimestamps.UpdateTimestamps;
        }

    }
}
