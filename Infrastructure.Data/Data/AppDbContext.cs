using Application.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetallesFactura { get; set; }
        public DbSet<AuditoriaFactura> AuditoriaFacturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("TR_Auditoria_Facturas"));
                entity.Property(f => f.InvoiceNumber).IsRequired().HasMaxLength(100);
                entity.Property(f => f.Customer).IsRequired().HasMaxLength(200);
                entity.Property(f => f.State).HasConversion<string>().HasMaxLength(20);
                entity.Property(f => f.Total).HasPrecision(18, 2);
            });

            modelBuilder.Entity<DetalleFactura>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("TR_Validar_Total_Factura"));
                entity.Property(d => d.Product).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Amount).IsRequired();
                entity.Property(d => d.UnitPrice).IsRequired().HasPrecision(18, 2);
                entity.Property(d => d.Subtotal).HasPrecision(18, 2);

                entity.HasOne(d => d.Bill)
                      .WithMany(f => f.Detail)
                      .HasForeignKey(d => d.IdBill)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleFactura>()
                .HasOne(d => d.Bill)
                .WithMany(f => f.Detail)
                .HasForeignKey(d => d.IdBill)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Factura>()
                .Property(f => f.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Factura>()
                .Property(f => f.Customer)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.Product)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.Amount)
                .IsRequired();

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.UnitPrice)
                .IsRequired()
                .HasPrecision(18,2);

            modelBuilder.Entity<Factura>()
                .Property(f => f.State)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Factura>()
                .Property(f => f.Total)
                .HasPrecision(18,2);

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.Subtotal)
                .HasPrecision(18,2);

            modelBuilder.Entity<AuditoriaFactura>(entity =>
            {
                entity.ToTable("AuditoriaFacturas");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Action).HasMaxLength(50);
                entity.Property(a => a.User).HasMaxLength(100);
            });

            modelBuilder.Entity<DetalleFactura>()
                .HasOne(d => d.Bill)
                .WithMany(f => f.Detail)
                .HasForeignKey(d => d.IdBill)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                options => options.EnableRetryOnFailure().UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            );
        }
    }
}
