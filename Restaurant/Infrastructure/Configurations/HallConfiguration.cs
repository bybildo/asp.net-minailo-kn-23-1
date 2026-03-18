using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Configurations
{
    public class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(h => h.Name)
                   .IsUnique();

            builder.Property(h => h.Width)
                   .IsRequired();

            builder.Property(h => h.Length)
                   .IsRequired();
        }
    }
}
