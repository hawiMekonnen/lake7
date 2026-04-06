using lake7.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }

}
