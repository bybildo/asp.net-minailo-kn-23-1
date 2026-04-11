using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Number)
                   .IsRequired();

            builder.Property(t => t.Capacity)
                   .IsRequired();

            // Явно вказуємо навігаційну властивість, щоб уникнути дублювання HallId / HallId1
            builder.HasOne(t => t.Hall)
                   .WithMany()
                   .HasForeignKey(t => t.HallId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
