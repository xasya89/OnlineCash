using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlineCash.DataBaseModels;
#nullable disable

namespace OnlineCash
{
    public partial class shopContext : DbContext
    {
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<BarCode> BarCodes { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<GoodPrice> GoodPrices { get; set; }
        public DbSet<GoodAdded> GoodAddeds { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftSale> ShiftSales { get; set; }
        public DbSet<CheckGood> CheckGoods { get; set; }
        public DbSet<CheckSell> CheckSells { get; set; }
        public DbSet<Stocktaking> Stocktakings { get; set; }
        public DbSet<StockTakingGroup> StockTakingGroups { get; set; }
        public DbSet<StocktakingGood> StocktakingGoods { get; set; }
        public DbSet<GoodBalance> GoodBalances { get; set; }
        public DbSet<GoodBalanceHistory> GoodBalanceHistories { get; set; }
        public DbSet<SumBalanceHistory> SumBalanceHistories { get; set; }
        public DbSet<Arrival> Arrivals { get; set; }
        public DbSet<ArrivalPayment> ArrivalPayments { get; set; }
        public DbSet<ArrivalGood> ArrivalGoods { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GoodGroup> GoodGroups { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Writeof> Writeofs { get; set; }
        public DbSet<WriteofGood> WriteofGoods { get; set; }
        public DbSet<MoveDoc> MoveDocs { get; set; }
        public DbSet<MoveGood> MoveGoods { get; set; }

        public DbSet<DiscountCard> DiscountCards { get; set; }
        public DbSet<Buyer> Buyers { get; set; }

        public shopContext()
        {
        }

        public shopContext(DbContextOptions<shopContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=172.172.172.120;database=shop;user=root;password=kt38hmapq;treattinyasboolean=true", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.30-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<Good>()
                .HasOne(g => g.Supplier)
                .WithMany(s => s.Goods)
                .HasForeignKey(g => g.SupplierId);
            modelBuilder.Entity<BarCode>()
                .HasOne(b => b.Good)
                .WithMany(g => g.BarCodes)
                .HasForeignKey(b => b.GoodId);
            modelBuilder.Entity<GoodPrice>()
                .HasOne(p => p.Shop)
                .WithMany(t => t.GoodPrices)
                .HasForeignKey(p => p.ShopId);
            modelBuilder.Entity<GoodPrice>()
                .HasOne(p => p.Good)
                .WithMany(p => p.GoodPrices)
                .HasForeignKey(p => p.GoodId);
            modelBuilder.Entity<GoodAdded>()
                .HasOne(g => g.Good)
                .WithMany(g => g.GoodAddeds)
                .HasForeignKey(g => g.GoodId);

            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Shop)
                .WithMany(s => s.Shifts)
                .HasForeignKey(s => s.ShopId);
            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Cashier)
                .WithMany(s => s.Shifts)
                .HasForeignKey(s => s.CashierId);
            modelBuilder.Entity<ShiftSale>()
                .HasOne(s => s.Shift)
                .WithMany(s => s.ShiftSales)
                .HasForeignKey(s => s.ShiftId);
            modelBuilder.Entity<ShiftSale>()
                .HasOne(s => s.Good)
                .WithMany(g => g.ShiftSales)
                .HasForeignKey(s => s.GoodId);

            modelBuilder.Entity<CheckGood>()
                .HasOne(c => c.CheckSell)
                .WithMany(s => s.CheckGoods)
                .HasForeignKey(c => c.CheckSellId);
            modelBuilder.Entity<CheckGood>()
                .HasOne(c => c.Good)
                .WithMany(g => g.CheckGoods)
                .HasForeignKey(c => c.GoodId);
            modelBuilder.Entity<CheckSell>()
                .HasOne(s => s.Shift)
                .WithMany(s => s.CheckSells)
                .HasForeignKey(s => s.ShiftId);
            //Инверторизация
            modelBuilder.Entity<StockTakingGroup>()
                .HasOne(s => s.Stocktaking)
                .WithMany(s => s.StockTakingGroups)
                .HasForeignKey(s => s.StocktakingId);
            modelBuilder.Entity<StocktakingGood>()
                .HasOne(s => s.StockTakingGroup)
                .WithMany(s => s.StocktakingGoods)
                .HasForeignKey(s => s.StockTakingGroupId);
            modelBuilder.Entity<StocktakingGood>()
                .HasOne(s => s.Good)
                .WithMany(g => g.StocktakingGoods)
                .HasForeignKey(s => s.GoodId);
            modelBuilder.Entity<Good>()
                .HasOne(g => g.GoodGroup)
                .WithMany(gr => gr.Goods)
                .HasForeignKey(g => g.GoodGroupId);

            //Поступления
            modelBuilder.Entity<ArrivalGood>()
                .HasOne(a => a.Good)
                .WithMany(g => g.ArrivalGoods)
                .HasForeignKey(a => a.GoodId);
            modelBuilder.Entity<ArrivalGood>()
                .HasOne(a => a.Arrival)
                .WithMany(a => a.ArrivalGoods)
                .HasForeignKey(a => a.ArrivalId);
            modelBuilder.Entity<Arrival>()
                .HasOne(a => a.Supplier)
                .WithMany(s => s.Arrivals)
                .HasForeignKey(a => a.SupplierId);
            modelBuilder.Entity<Arrival>()
                .HasOne(a => a.Shop)
                .WithMany(s => s.Arrivals)
                .HasForeignKey(a => a.ShopId);
            modelBuilder.Entity<ArrivalPayment>()
                .HasOne(p => p.Arrival)
                .WithMany(a => a.ArrivalPayments)
                .HasForeignKey(p => p.ArrivalId);
            modelBuilder.Entity<ArrivalPayment>()
                .HasOne(p => p.BankAccount)
                .WithMany(b => b.ArrivalPayments)
                .HasForeignKey(p => p.BankAccountId);

            //Списание
            modelBuilder.Entity<Writeof>()
                .HasOne(w => w.Shop)
                .WithMany(s => s.Writeofs)
                .HasForeignKey(w => w.ShopId);
            modelBuilder.Entity<WriteofGood>()
                .HasOne(wg => wg.Writeof)
                .WithMany(w => w.WriteofGoods)
                .HasForeignKey(wg => wg.WriteofId);
            modelBuilder.Entity<WriteofGood>()
                .HasOne(wg => wg.Good)
                .WithMany(g => g.WriteofGoods)
                .HasForeignKey(wg => wg.GoodId);
            //Перемещение между магазинами
            modelBuilder.Entity<MoveDoc>()
                .HasOne(m => m.ConsigneeShop)
                .WithMany(s => s.MoveConsigneeDocs)
                .HasForeignKey(m => m.ConsigneeShopId);
            modelBuilder.Entity<MoveDoc>()
                .HasOne(m => m.ConsignerShop)
                .WithMany(s => s.MoveConsignerDocs)
                .HasForeignKey(m => m.ConsignerShopId);
            modelBuilder.Entity<MoveGood>()
                .HasOne(m => m.Good)
                .WithMany(g => g.MoveGoods)
                .HasForeignKey(m => m.GoodId);
            modelBuilder.Entity<MoveGood>()
                .HasOne(m => m.MoveDoc)
                .WithMany(d => d.MoveGoods)
                .HasForeignKey(m => m.MoveDocId);
            //GoodBalance
            modelBuilder.Entity<GoodBalanceHistory>()
                .HasOne(b => b.Good)
                .WithMany(g => g.GoodBalanceHistories)
                .HasForeignKey(b => b.GoodId);
            modelBuilder.Entity<GoodBalanceHistory>()
                .HasOne(b => b.Shop)
                .WithMany(g => g.GoodBalanceHistories)
                .HasForeignKey(g => g.ShopId);

            //SumBalance
            modelBuilder.Entity<SumBalanceHistory>()
                .HasOne(b => b.Shop)
                .WithMany(s => s.SumBalanceHistories)
                .HasForeignKey(b => b.ShopId);

            modelBuilder.Entity<Buyer>()
                .HasOne(b => b.DiscountCard)
                .WithMany(d => d.Buyers)
                .HasForeignKey(b=>b.DiscountCardId);
            modelBuilder.Entity<CheckSell>()
                .HasOne(c => c.Buyer)
                .WithMany(b => b.CheckSells)
                .HasForeignKey(b => b.BuyerId);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
