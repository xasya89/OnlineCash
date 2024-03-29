﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RMK;

namespace RMK.Migrations
{
    [DbContext(typeof(onlinecashContext))]
    [Migration("20210424193848_AlterChecSell_AddRelationToShift")]
    partial class AlterChecSell_AddRelationToShift
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataBase.Cashier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Inn")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PinCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Cashiers");
                });

            modelBuilder.Entity("DataBase.CheckGood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CheckSellId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(65,30)");

                    b.Property<double>("Count")
                        .HasColumnType("double");

                    b.Property<int>("GoodId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CheckSellId");

                    b.HasIndex("GoodId");

                    b.ToTable("CheckGoods");
                });

            modelBuilder.Entity("DataBase.CheckSell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsElectron")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ShiftId")
                        .HasColumnType("int");

                    b.Property<decimal>("Sum")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("SumAll")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("SumDiscont")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId");

                    b.ToTable("CheckSells");
                });

            modelBuilder.Entity("DataBase.Good", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Article")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BarCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("DataBase.GoodPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("GoodId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoodId");

                    b.HasIndex("ShopId");

                    b.ToTable("GoodPrices");
                });

            modelBuilder.Entity("DataBase.Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CashierId")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Stop")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("SumAll")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("SumIncome")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("SumOutcome")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("SumSell")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("SummReturn")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("CashierId");

                    b.HasIndex("ShopId");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("DataBase.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("DataBase.CheckGood", b =>
                {
                    b.HasOne("DataBase.CheckSell", "CheckSell")
                        .WithMany("CheckGoods")
                        .HasForeignKey("CheckSellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Good", "Good")
                        .WithMany("CheckGoods")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataBase.CheckSell", b =>
                {
                    b.HasOne("DataBase.Shift", "Shift")
                        .WithMany("CheckSells")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataBase.GoodPrice", b =>
                {
                    b.HasOne("DataBase.Good", "Good")
                        .WithMany("GoodPrices")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Shop", "Shop")
                        .WithMany("GoodPrices")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataBase.Shift", b =>
                {
                    b.HasOne("DataBase.Cashier", "Cashier")
                        .WithMany("Shifts")
                        .HasForeignKey("CashierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Shop", "Shop")
                        .WithMany("Shifts")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
