using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mini_GPT.Models;

namespace Mini_GPT.Data
{
    public class AppSqlDBContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> users { get; set; }

        public AppSqlDBContext(DbContextOptions<AppSqlDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "user",
                    NormalizedName = "USER"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
            // Apply configurations for entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppSqlDBContext).Assembly);
        }


    }
}
