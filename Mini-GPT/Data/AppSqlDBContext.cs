using Microsoft.EntityFrameworkCore;
using Mini_GPT.Models;

namespace Mini_GPT.Data
{
    public class AppSqlDBContext : DbContext
    {
        public DbSet<User> users { get; set; }

        public AppSqlDBContext(DbContextOptions<AppSqlDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations for entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppSqlDBContext).Assembly);
        }


    }
}
