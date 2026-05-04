using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Identity;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Configurations;

namespace Restaurant.Infrastructure
{
    // Команди для міграцій (запускати з теки Presentation):
    // dotnet ef migrations add InitialCreate --project ../Infrastructure
    // dotnet ef database update --project ../Infrastructure

    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public new DbSet<User> Users { get; set; } = null!;
        public DbSet<Hall> Halls { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Table> Tables { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new HallConfiguration());
            modelBuilder.ApplyConfiguration(new TableConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        }
    }
}
