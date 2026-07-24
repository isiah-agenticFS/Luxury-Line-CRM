using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScrubCRM.Models;

namespace ScrubCRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrderRequest> OrderRequests { get; set; }
        public DbSet<OrderRequestItem> OrderRequestItems { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            modelBuilder.Entity<ProductVariant>()
                .HasIndex(v => v.VariantSku)
                .IsUnique();

            modelBuilder.Entity<ProductVariant>()
                .HasRequired(v => v.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(v => v.ProductId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InventoryTransaction>()
                .HasRequired(t => t.ProductVariant)
                .WithMany(v => v.InventoryTransactions)
                .HasForeignKey(t => t.ProductVariantId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InventoryTransaction>()
                .HasRequired(t => t.Administrator)
                .WithMany()
                .HasForeignKey(t => t.AdministratorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderRequest>()
                .HasIndex(o => o.RequestNumber)
                .IsUnique();

            modelBuilder.Entity<OrderRequest>()
                .HasOptional(o => o.Customer)
                .WithMany(c => c.OrderRequests)
                .HasForeignKey(o => o.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderRequest>()
                .HasOptional(o => o.Organization)
                .WithMany(o => o.OrderRequests)
                .HasForeignKey(o => o.OrganizationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderRequestItem>()
                .HasRequired(i => i.OrderRequest)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderRequestId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderRequestItem>()
                .HasRequired(i => i.ProductVariant)
                .WithMany(v => v.OrderRequestItems)
                .HasForeignKey(i => i.ProductVariantId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderStatusHistory>()
                .HasRequired(h => h.OrderRequest)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(h => h.OrderRequestId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderStatusHistory>()
                .HasRequired(h => h.Administrator)
                .WithMany()
                .HasForeignKey(h => h.AdministratorId)
                .WillCascadeOnDelete(false);
        }
    }
}
