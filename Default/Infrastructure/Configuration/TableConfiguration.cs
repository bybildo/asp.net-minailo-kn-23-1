using Default.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Default.Data.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Number).IsRequired();
            builder.Property(t => t.Capacity).IsRequired();

            builder.HasOne<Hall>()
                   .WithMany()
                   .HasForeignKey(t => t.HallId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}