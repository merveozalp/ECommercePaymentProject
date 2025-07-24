using Microsoft.EntityFrameworkCore;
using ECommercePayment.Domain.Entities;
using System;

namespace ECommercePayment.Infrastructure.Persistence
{
    public class ECommerceDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product Entity Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Currency).HasMaxLength(10);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Timestamp).IsRequired();
            });

            // Order Entity Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(e => e.ProductId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.HasMany(e => e.Items)
                      .WithOne()
                      .HasForeignKey("OrderId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem Entity Configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(e => e.ProductId).HasMaxLength(50);
                entity.Property(e => e.ProductName).HasMaxLength(200);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Timestamp).IsRequired();
            });

            // Seed Data - Mevcut product verileriniz
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = "prod-001",
                    Name = "Premium Smartphone",
                    Description = "Latest model with advanced features",
                    Price = 19.99m,
                    Currency = "USD",
                    Category = "Electronics",
                    Stock = 42,
                    Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = "prod-002",
                    Name = "Wireless Headphones",
                    Description = "Noise-cancelling with premium sound quality",
                    Price = 14.99m,
                    Currency = "USD",
                    Category = "Electronics",
                    Stock = 78,
                    Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = "prod-003",
                    Name = "Smart Watch",
                    Description = "Fitness tracking and notifications",
                    Price = 12.99m,
                    Currency = "USD",
                    Category = "Electronics",
                    Stock = 0,
                    Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = "prod-004",
                    Name = "Laptop",
                    Description = "High-performance for work and gaming",
                    Price = 19.99m,
                    Currency = "USD",
                    Category = "Electronics",
                    Stock = 15,
                    Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = "prod-005",
                    Name = "Wireless Charger",
                    Description = "Fast charging for compatible devices",
                    Price = 9.99m,
                    Currency = "USD",
                    Category = "Accessories",
                    Stock = 120,
                    Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
} 