using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.StartDate)
                   .IsRequired();

            builder.Property(r => r.EndDate)
                   .IsRequired();

            builder.HasMany(r => r.Tables)
                   .WithMany()
                   .UsingEntity<Dictionary<string, object>>("ReservationTables",
                       j => j.HasOne<Table>().WithMany().HasForeignKey("TableId"),
                       j => j.HasOne<Reservation>().WithMany().HasForeignKey("ReservationId"));
        }
    }
}
