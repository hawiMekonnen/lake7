using lake7.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace lake7.Infrastructure.Context
{
    public class Lake7DbContext : DbContext
    {
        public Lake7DbContext(DbContextOptions<Lake7DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Store RideStatus enum as string in DB
            modelBuilder.Entity<Ride>()
                .Property(r => r.Status)
                .HasConversion<string>();

            // Configure Payment.Amount with precision to avoid truncation warnings
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DriverLocation> DriverLocations { get; set; }
    }
}
