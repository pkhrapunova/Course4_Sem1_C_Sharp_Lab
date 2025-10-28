using CarRental.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Web.Models
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
           : base(options)
        { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Car
            modelBuilder.Entity<Car>()
                .HasKey(e => e.CarID);

            modelBuilder.Entity<Car>()
                .Property(e => e.CarNumber)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Car>()
                .Property(e => e.Make)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Car>()
                .Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Car>()
                .Property(e => e.PricePerHour)
                .HasPrecision(18, 2);

            // Customer
            modelBuilder.Entity<Customer>()
                .HasKey(e => e.CustomerID);

            modelBuilder.Entity<Customer>()
                .Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);

            // Order и связи
            modelBuilder.Entity<Order>()
                .HasKey(e => e.OrderID);

            // Один-ко-многим: Customer -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID);

            // Один-ко-многим: Car -> Orders  
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Car)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CarID);

        }
    }
}