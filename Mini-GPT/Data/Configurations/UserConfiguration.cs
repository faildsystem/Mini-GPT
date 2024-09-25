using Microsoft.EntityFrameworkCore;
using Mini_GPT.Models;

namespace Mini_GPT.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppUser> builder)
        {
           
            builder
                .Property(u => u.ProfileImage)
                .IsRequired(false);
        }
    }
}
