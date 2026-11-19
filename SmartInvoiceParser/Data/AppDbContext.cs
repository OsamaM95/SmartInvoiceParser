using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using SmartInvoiceParser.Models;

namespace SmartInvoiceParser.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>().Property(i => i.SubTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Invoice>().Property(i => i.TaxAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Invoice>().Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<InvoiceItem>().Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<InvoiceItem>().Property(i => i.TotalPrice).HasColumnType("decimal(18,2)");
        }
    }
}