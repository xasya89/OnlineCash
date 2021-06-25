using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataBase;
using RMK.DataBaseModels;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RMK
{
    public partial class onlinecashContext : DbContext
    {
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<GoodPrice> GoodPrices { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CheckGood> CheckGoods { get; set; }
        public DbSet<CheckSell> CheckSells { get; set; }
        public DbSet<Stocktaking> Stocktakings { get; set; }
        public DbSet<StocktakingGood> StocktakingGoods { get; set; }
        public onlinecashContext()
        {
            Database.SetCommandTimeout(60);
        }

        public onlinecashContext(DbContextOptions<onlinecashContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(60);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=172.172.172.120;database=rmk;uid=root;pwd=kt38hmapq", x => x.ServerVersion("5.7.30-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoodPrice>()
                .HasOne(p => p.Shop)
                .WithMany(t => t.GoodPrices)
                .HasForeignKey(p => p.ShopId);
            modelBuilder.Entity<GoodPrice>()
                .HasOne(p => p.Good)
                .WithMany(p => p.GoodPrices)
                .HasForeignKey(p => p.GoodId);

            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Shop)
                .WithMany(s => s.Shifts)
                .HasForeignKey(s => s.ShopId);
            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Cashier)
                .WithMany(s => s.Shifts)
                .HasForeignKey(s => s.CashierId);

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

            modelBuilder.Entity<StocktakingGood>()
                .HasOne(s => s.Stocktaking)
                .WithMany(s => s.StocktakingGoods)
                .HasForeignKey(s => s.StocktakingId);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
