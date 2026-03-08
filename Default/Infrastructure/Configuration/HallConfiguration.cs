using Default.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Default.Infrastructure.Configuration
{
    public class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {
            builder.HasKey(h => h.Id);
            builder.HasIndex(h => h.Name).IsUnique();
            builder.Property(h => h.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(h => h.Width).IsRequired();
            builder.Property(h => h.Length).IsRequired();
        }
    }
}
