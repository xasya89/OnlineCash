﻿// <auto-generated />
using System;
using DatabaseBuyer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabaseBuyer.Migrations
{
    [DbContext(typeof(shopbuyerContext))]
    [Migration("20220619115346_AlterBuyer_Add_SumDiscountInBuy")]
    partial class AlterBuyer_Add_SumDiscountInBuy
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci")
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.16");

            modelBuilder.Entity("DatabaseBuyer.Buyer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("DiscountSum")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<int>("SpecialPercent")
                        .HasColumnType("int");

                    b.Property<decimal>("SumBuy")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("SumDiscountInBuy")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("TemporyPercent")
                        .HasColumnType("int");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("char(36)");

                    b.Property<bool>("isBlock")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("DatabaseBuyer.DiscountSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Settings")
                        .HasColumnType("json");

                    b.HasKey("Id");

                    b.ToTable("DiscountSettings");
                });
#pragma warning restore 612, 618
        }
    }
}
