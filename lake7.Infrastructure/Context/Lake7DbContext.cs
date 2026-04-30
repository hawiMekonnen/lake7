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

            // Enum conversions
            modelBuilder.Entity<Ride>().Property(r => r.Status).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(o => o.Status).HasConversion<string>();
            modelBuilder.Entity<Payment>().Property(p => p.Status).HasConversion<string>();

            // Precision
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            // ==================== CRITICAL FIX FOR CASCADE ERROR ====================

            // 1. Payment to Order - FORCE Restrict (this is the main problematic one)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict)     // This should prevent cascade
                .IsRequired(true);                     // Make sure it's required

            // 2. Payment to User
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // 3. Optional relationships - No cascade
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Ride)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.RideId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Delivery)
                .WithMany(d => d.Payments)
                .HasForeignKey(p => p.DeliveryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // 4. Order to Ride & Delivery
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Ride)
                .WithMany()
                .HasForeignKey(o => o.RideId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Delivery)
                .WithMany()
                .HasForeignKey(o => o.DeliveryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // Extra safety
            modelBuilder.Entity<Ride>()
                .HasMany(r => r.Payments)
                .WithOne(p => p.Ride)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Delivery>()
                .HasMany(d => d.Payments)
                .WithOne(p => p.Delivery)
                .OnDelete(DeleteBehavior.NoAction);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DriverLocation> DriverLocations { get; set; }
    }
}
