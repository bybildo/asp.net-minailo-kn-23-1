using Default.Models;
using Default.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Default.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Login)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.PasswordHash).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Role).IsRequired();
        }
    }
}