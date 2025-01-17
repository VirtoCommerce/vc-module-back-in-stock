﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtoCommerce.BackInStock.Data.Repositories;

#nullable disable

namespace VirtoCommerce.BackInStock.Data.MySql.Migrations
{
    [DbContext(typeof(BackInStockDbContext))]
    [Migration("20250116144455_AddBackInStockSubscription")]
    partial class AddBackInStockSubscription
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("VirtoCommerce.BackInStock.Data.Models.BackInStockSubscriptionEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MemberId")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProductName")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("ProductId", "IsActive");

                    b.HasIndex("UserId", "ProductId", "StoreId", "IsActive")
                        .IsUnique();

                    b.ToTable("BackInStockSubscription", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
