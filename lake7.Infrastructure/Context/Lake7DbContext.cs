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
            
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

          
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict)     // This should prevent cascade
                .IsRequired(true);                     // Make sure it's required

            
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            
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

            //cd Order to Ride & Delivery
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

            // Seed Admin
            modelBuilder.Entity<AdminAccount>().HasData(new AdminAccount {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                FullName = "System Admin",
                Email = "admin@lake7.com",
                Password = "admin123", // this would be hashed
                Role = "SuperAdmin",
                CreatedAt = new DateTime(2024, 5, 8),
                UpdatedAt = new DateTime(2024, 5, 8),
                LastLogin = new DateTime(2024, 5, 8)
            });
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DriverLocation> DriverLocations { get; set; }
        public DbSet<AdminAccount> AdminAccounts { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
    }
}
