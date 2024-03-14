﻿// <auto-generated />
using Fashion.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fashion.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240310215308_AddInitialModels")]
    partial class AddInitialModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Fashion.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Clothes"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Shoes"
                        });
                });

            modelBuilder.Entity("Fashion.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "This is Dress",
                            ImageUrl = "https://i.pinimg.com/236x/d1/8e/f3/d18ef3e698b7c5822060aa9572bd5105.jpg",
                            Price = 10.99m,
                            Title = "Dress"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Description = "This is T-Shirt",
                            ImageUrl = "https://i.pinimg.com/236x/28/9e/0d/289e0dc040aca1d87f0536e43969b15a.jpg",
                            Price = 7.99m,
                            Title = "T-Shirt"
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 2,
                            Description = "This is Shoes",
                            ImageUrl = "https://i.pinimg.com/736x/c6/ef/fb/c6effbdce6a7da900fda401a52d15d96.jpg",
                            Price = 8.99m,
                            Title = "Shoes"
                        });
                });

            modelBuilder.Entity("Fashion.Models.Product", b =>
                {
                    b.HasOne("Fashion.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Fashion.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
