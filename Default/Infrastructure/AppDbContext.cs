using Default.Data.Configurations;
using Default.Infrastructure.Configuration;
using Default.Models;
using Microsoft.EntityFrameworkCore;

namespace Default.Infrastructure
{
    //dotnet ef migrations add CreateMigrationDatabase
    //dotnet ef database update

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new HallConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new TableConfiguration());
        }
    }
}
