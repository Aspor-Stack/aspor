using Microsoft.EntityFrameworkCore;

namespace Test.Model
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DbSet<Project> Projects { get; set; }
    }
}
