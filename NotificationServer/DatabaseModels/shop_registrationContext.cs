using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NotificationServer.DatabaseModels
{
    public partial class shop_registrationContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<NotificationClient> NotificationClients { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<NotifySubscribe> NotifySubscribes { get; set; }

        public shop_registrationContext()
        {
        }

        public shop_registrationContext(DbContextOptions<shop_registrationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=shop_registration;uid=root;pwd=kt38hmapq", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            modelBuilder.Entity<Shop>()
                .HasOne(s => s.Organization)
                .WithMany(o => o.Shops)
                .HasForeignKey(s => s.OrganizationId);

            modelBuilder.Entity<NotifySubscribe>()
                .HasOne(s => s.NotificationClient)
                .WithMany(c => c.NotifySubscribes)
                .HasForeignKey(s=>s.NotificationClientId);

            modelBuilder.Entity<NotifySubscribe>()
                .HasOne(s => s.Organization)
                .WithMany(o => o.NotifySubscribes)
                .HasForeignKey(s => s.OrganizationId);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
