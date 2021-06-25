﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineCash;

namespace OnlineCash.Migrations
{
    [DbContext(typeof(shopContext))]
    partial class shopContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci")
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("OnlineCash.DataBaseModels.Arrival", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("CountAll")
                        .HasColumnType("double");

                    b.Property<DateTime>("DateArrival")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Num")
                        .HasColumnType("longtext");

                    b.Property<decimal>("PriceAll")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<bool>("isSuccess")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Arrivals");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.ArrivalGood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ArrivalId")
                        .HasColumnType("int");

                    b.Property<double>("Count")
                        .HasColumnType("double");

                    b.Property<int>("GoodId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("ArrivalId");

                    b.HasIndex("GoodId");

                    b.ToTable("ArrivalGoods");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Cashier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Inn")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("PinCode")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Cashiers");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.CheckGood", b =>
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

            modelBuilder.Entity("OnlineCash.DataBaseModels.CheckSell", b =>
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

            modelBuilder.Entity("OnlineCash.DataBaseModels.Good", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Article")
                        .HasColumnType("longtext");

                    b.Property<string>("BarCode")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.GoodAdded", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("GoodId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoodId");

                    b.ToTable("GoodAddeds");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.GoodBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Count")
                        .HasColumnType("double");

                    b.Property<int>("GoodId")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoodId");

                    b.HasIndex("ShopId");

                    b.ToTable("GoodBalances");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.GoodPrice", b =>
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

            modelBuilder.Entity("OnlineCash.DataBaseModels.Shift", b =>
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

            modelBuilder.Entity("OnlineCash.DataBaseModels.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Stocktaking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Create")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Num")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<bool>("isSuccess")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("Stocktakings");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.StocktakingGood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Count")
                        .HasColumnType("double");

                    b.Property<double>("CountDB")
                        .HasColumnType("double");

                    b.Property<double>("CountFact")
                        .HasColumnType("double");

                    b.Property<int>("GoodId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("StocktakingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoodId");

                    b.HasIndex("StocktakingId");

                    b.ToTable("StocktakingGoods");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Inn")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Arrival", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Shop", "Shop")
                        .WithMany("Arrivals")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCash.DataBaseModels.Supplier", "Supplier")
                        .WithMany("Arrivals")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.ArrivalGood", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Arrival", "Arrival")
                        .WithMany("ArrivalGoods")
                        .HasForeignKey("ArrivalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCash.DataBaseModels.Good", "Good")
                        .WithMany("ArrivalGoods")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Arrival");

                    b.Navigation("Good");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.CheckGood", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.CheckSell", "CheckSell")
                        .WithMany("CheckGoods")
                        .HasForeignKey("CheckSellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCash.DataBaseModels.Good", "Good")
                        .WithMany("CheckGoods")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckSell");

                    b.Navigation("Good");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.CheckSell", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Shift", "Shift")
                        .WithMany("CheckSells")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.GoodAdded", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Good", "Good")
                        .WithMany("GoodAddeds")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Good");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.GoodBalance", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Good", "Good")
                        .WithMany("GoodBalances")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCash.DataBaseModels.Shop", "Shop")
                        .WithMany("GoodBalances")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Good");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.GoodPrice", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Good", "Good")
                        .WithMany("GoodPrices")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCash.DataBaseModels.Shop", "Shop")
                        .WithMany("GoodPrices")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Good");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Shift", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Cashier", "Cashier")
                        .WithMany("Shifts")
                        .HasForeignKey("CashierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCash.DataBaseModels.Shop", "Shop")
                        .WithMany("Shifts")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cashier");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Stocktaking", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.StocktakingGood", b =>
                {
                    b.HasOne("OnlineCash.DataBaseModels.Good", "Good")
                        .WithMany("StocktakingGoods")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCash.DataBaseModels.Stocktaking", "Stocktaking")
                        .WithMany("StocktakingGoods")
                        .HasForeignKey("StocktakingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Good");

                    b.Navigation("Stocktaking");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Arrival", b =>
                {
                    b.Navigation("ArrivalGoods");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Cashier", b =>
                {
                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.CheckSell", b =>
                {
                    b.Navigation("CheckGoods");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Good", b =>
                {
                    b.Navigation("ArrivalGoods");

                    b.Navigation("CheckGoods");

                    b.Navigation("GoodAddeds");

                    b.Navigation("GoodBalances");

                    b.Navigation("GoodPrices");

                    b.Navigation("StocktakingGoods");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Shift", b =>
                {
                    b.Navigation("CheckSells");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Shop", b =>
                {
                    b.Navigation("Arrivals");

                    b.Navigation("GoodBalances");

                    b.Navigation("GoodPrices");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Stocktaking", b =>
                {
                    b.Navigation("StocktakingGoods");
                });

            modelBuilder.Entity("OnlineCash.DataBaseModels.Supplier", b =>
                {
                    b.Navigation("Arrivals");
                });
#pragma warning restore 612, 618
        }
    }
}
