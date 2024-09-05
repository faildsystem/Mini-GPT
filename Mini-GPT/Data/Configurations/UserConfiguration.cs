using Microsoft.EntityFrameworkCore;
using Mini_GPT.Models;

namespace Mini_GPT.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .Property(u => u.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(u => u.Password)
                .HasMaxLength(265)
                .IsRequired();

            builder
                .Property(u => u.ProfileImage)
                .IsRequired(false);

            builder
                .Property(u => u.CreatedAt)
                .IsRequired();

            builder
                .Property(u => u.LastLogin)
                .IsRequired(false);


            builder
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
