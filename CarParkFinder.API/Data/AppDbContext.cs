using Microsoft.EntityFrameworkCore;
using CarParkFinder.API.Models;

namespace CarParkFinder.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CarPark> CarParks { get; set; }
        public DbSet<CarParkAvailability> CarParkAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints

            modelBuilder.Entity<CarPark>()
                .HasMany(c => c.Availabilities)
                .WithOne(a => a.CarPark)
                .HasForeignKey(a => a.CarParkId);

            modelBuilder.Entity<CarPark>()
                .HasIndex(c => c.CarParkNo)
                .IsUnique();

            modelBuilder.Entity<CarParkAvailability>()
                .HasIndex(a => new { a.CarParkNo, a.RetrievedAt })
                .IsUnique();
        }
    }
}
