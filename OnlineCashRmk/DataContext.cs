using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    public class DataContext:DbContext
    {
        public DbSet<DocSynch> DocSynches { get; set; }

        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<BarCode> BarCodes { get; set; }
        public DbSet<CheckSell> CheckSells { get; set; }
        public DbSet<CheckGood> CheckGoods { get; set; }
        public DbSet<CheckPayment> CheckPayments { get; set; }

        public DbSet<Credit> Credits { get; set; }
        public DbSet<CreditGood> CreditGoods { get; set; }
        public DbSet<CreditPayment> CreditPayments { get; set; }

        public DbSet<Writeof> Writeofs { get; set; }
        public DbSet<WriteofGood> WriteofGoods { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Arrival> Arrivals { get; set; }
        public DbSet<ArrivalGood> ArrivalGoods { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<Stocktaking> Stocktakings { get; set; }
        public DbSet<StocktakingGroup> StocktakingGroups { get; set; }
        public DbSet<StocktakingGood> StocktakingGoods { get; set; }

        public DbSet<CashMoney> CashMoneys { get; set; }

        public DbSet<NewGoodFromCash> NewGoodsFromCash { get; set; }

        public DbSet<Revaluation> Revaluations { get; set; }
        public DbSet<RevaluationGood> RevaluationGoods { get; set; }

        public DbSet<DiscountSetting> DiscountSettings { get; set; }
        
        public DataContext() 
        {
        }
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;database=shop_rmk;uid=root;pwd=kt38hmapq", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.30-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shift>().ToTable("Shifts");
            modelBuilder.Entity<Good>().ToTable("Goods");
            modelBuilder.Entity<BarCode>()
                .HasOne(b => b.Good)
                .WithMany(g => g.BarCodes)
                .HasForeignKey(b => b.GoodId);
            modelBuilder.Entity<CheckGood>().ToTable("CheckGoods");
            modelBuilder.Entity<CheckSell>().ToTable("CheckSells");

            modelBuilder.Entity<CheckGood>()
                .HasOne(c => c.Good)
                .WithMany(g => g.CheckGoods)
                .HasForeignKey(c => c.GoodId);
            modelBuilder.Entity<CheckGood>()
                .HasOne(c => c.CheckSell)
                .WithMany(s => s.CheckGoods)
                .HasForeignKey(c => c.CheckSellId);
            modelBuilder.Entity<CheckSell>()
                .HasOne(s => s.Shift)
                .WithMany(s => s.CheckSells)
                .HasForeignKey(s => s.ShiftId);
            modelBuilder.Entity<CheckPayment>()
                .HasOne(p => p.CheckSell)
                .WithMany(s => s.CheckPayments)
                .HasForeignKey(p => p.CheckSellId);

            modelBuilder.Entity<CreditGood>()
                .HasOne(ch => ch.Credit)
                .WithMany(c => c.CreditGoods)
                .HasForeignKey(ch => ch.CreditId);
            modelBuilder.Entity<CreditPayment>()
                .HasOne(p => p.Credit)
                .WithMany(c => c.CreditPayments)
                .HasForeignKey(p => p.CreditId);
            modelBuilder.Entity<Credit>()
                .HasOne(c => c.Shift)
                .WithMany(s => s.Credits)
                .HasForeignKey(c => c.ShiftId);

            modelBuilder.Entity<WriteofGood>()
                .HasOne(w => w.Writeof)
                .WithMany(w => w.WriteofGoods)
                .HasForeignKey(w => w.WriteofId);

            modelBuilder.Entity<Arrival>()
                .HasOne(a => a.Supplier)
                .WithMany(s => s.Arrivals)
                .HasForeignKey(a => a.SupplierId);
            modelBuilder.Entity<ArrivalGood>()
                .HasOne(a => a.Arrival)
                .WithMany(a => a.ArrivalGoods)
                .HasForeignKey(a => a.ArrivalId);
            modelBuilder.Entity<ArrivalGood>()
                .HasOne(a => a.Good)
                .WithMany(a => a.ArrivalGoods)
                .HasForeignKey(a => a.GoodId);

            modelBuilder.Entity<StocktakingGroup>()
                .HasOne(gr => gr.Stocktaking)
                .WithMany(s => s.StocktakingGroups)
                .HasForeignKey(gr => gr.StocktakingId);
            modelBuilder.Entity<StocktakingGood>()
                .HasOne(g => g.StocktakingGroup)
                .WithMany(gr => gr.StocktakingGoods)
                .HasForeignKey(g => g.StocktakingGroupId);
            modelBuilder.Entity<StocktakingGood>()
                .HasOne(s => s.Good)
                .WithMany(g => g.StocktakingGoods)
                .HasForeignKey(s => s.GoodId);

            modelBuilder.Entity<NewGoodFromCash>()
                .HasOne(n => n.Good)
                .WithMany(g => g.NewGoodsFromCashe)
                .HasForeignKey(n => n.GoodId);

            modelBuilder.Entity<RevaluationGood>()
                .HasOne(r => r.Revaluation)
                .WithMany(r => r.RevaluationGoods)
                .HasForeignKey(r => r.RevaluationId);
            modelBuilder.Entity<RevaluationGood>()
                .HasOne(r => r.Good)
                .WithMany(r => r.RevaluationGoods)
                .HasForeignKey(r => r.GoodId);
        }
    }
}
