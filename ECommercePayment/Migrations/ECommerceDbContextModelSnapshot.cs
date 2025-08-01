﻿// <auto-generated />
using System;
using ECommercePayment.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ECommercePayment.Migrations
{
    [DbContext(typeof(ECommerceDbContext))]
    partial class ECommerceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ECommercePayment.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ECommercePayment.Domain.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ECommercePayment.Domain.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = "prod-001",
                            Category = "Electronics",
                            Currency = "USD",
                            Description = "Latest model with advanced features",
                            Name = "Premium Smartphone",
                            Price = 19.99m,
                            Stock = 42,
                            Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = "prod-002",
                            Category = "Electronics",
                            Currency = "USD",
                            Description = "Noise-cancelling with premium sound quality",
                            Name = "Wireless Headphones",
                            Price = 14.99m,
                            Stock = 78,
                            Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = "prod-003",
                            Category = "Electronics",
                            Currency = "USD",
                            Description = "Fitness tracking and notifications",
                            Name = "Smart Watch",
                            Price = 12.99m,
                            Stock = 0,
                            Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = "prod-004",
                            Category = "Electronics",
                            Currency = "USD",
                            Description = "High-performance for work and gaming",
                            Name = "Laptop",
                            Price = 19.99m,
                            Stock = 15,
                            Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = "prod-005",
                            Category = "Accessories",
                            Currency = "USD",
                            Description = "Fast charging for compatible devices",
                            Name = "Wireless Charger",
                            Price = 9.99m,
                            Stock = 120,
                            Timestamp = new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("ECommercePayment.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("ECommercePayment.Domain.Entities.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ECommercePayment.Domain.Entities.Order", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
