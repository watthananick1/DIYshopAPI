﻿// <auto-generated />
using System;
using DIYshopAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DIYshopAPI.Migrations.Promotion
{
    [DbContext(typeof(PromotionContext))]
    [Migration("20230705081418_addPromotionDb")]
    partial class addPromotionDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DIYshopAPI.Models.Promotion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("IdProduct_One")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdProduct_Two")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameProduct_One")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameProduct_Two")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Promotions");
                });
#pragma warning restore 612, 618
        }
    }
}
