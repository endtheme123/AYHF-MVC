﻿// <auto-generated />
using AYHF.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AYHF.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240405184630_i wanna die")]
    partial class iwannadie
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AYHF.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Action"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Action"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Action"
                        });
                });

            modelBuilder.Entity("AYHF.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Ordered")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "fresh and yummy",
                            Name = "potato",
                            Ordered = 0,
                            Price = 5.0,
                            Stock = 100
                        },
                        new
                        {
                            Id = 2,
                            Description = "fresh and yummy",
                            Name = "tomato",
                            Ordered = 0,
                            Price = 5.0,
                            Stock = 100
                        },
                        new
                        {
                            Id = 3,
                            Description = "tasty and yummy",
                            Name = "healthy burger",
                            Ordered = 0,
                            Price = 5.0,
                            Stock = 100
                        },
                        new
                        {
                            Id = 4,
                            Description = "fresh and yummy",
                            Name = "carrot",
                            Ordered = 0,
                            Price = 5.0,
                            Stock = 100
                        },
                        new
                        {
                            Id = 5,
                            Description = "fresh and yummy",
                            Name = "onion",
                            Ordered = 0,
                            Price = 5.0,
                            Stock = 100
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
