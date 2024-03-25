using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAPIBanco.Modelos;

namespace WebAPIBanco;

public partial class WebAppBancoContextContext : DbContext
{
    public WebAppBancoContextContext()
    {
    }

    public WebAppBancoContextContext(DbContextOptions<WebAppBancoContextContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Tarjeta> Tarjetas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:WebAppBancoContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

            entity.HasMany(d => d.Tarjetas).WithMany(p => p.Compras)
                .UsingEntity<Dictionary<string, object>>(
                    "ComprasTarjeta",
                    r => r.HasOne<Tarjeta>().WithMany().HasForeignKey("TarjetasId"),
                    l => l.HasOne<Compra>().WithMany().HasForeignKey("ComprasId"),
                    j =>
                    {
                        j.HasKey("ComprasId", "TarjetasId");
                        j.ToTable("ComprasTarjetas");
                        j.HasIndex(new[] { "TarjetasId" }, "IX_ComprasTarjetas_TarjetasId");
                    });
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

            entity.HasMany(d => d.Tarjetas).WithMany(p => p.Pagos)
                .UsingEntity<Dictionary<string, object>>(
                    "PagosTarjeta",
                    r => r.HasOne<Tarjeta>().WithMany().HasForeignKey("TarjetasId"),
                    l => l.HasOne<Pago>().WithMany().HasForeignKey("PagosId"),
                    j =>
                    {
                        j.HasKey("PagosId", "TarjetasId");
                        j.ToTable("PagosTarjetas");
                        j.HasIndex(new[] { "TarjetasId" }, "IX_PagosTarjetas_TarjetasId");
                    });
        });

        modelBuilder.Entity<Tarjeta>(entity =>
        {
            entity.Property(e => e.Apellidos).HasDefaultValue("");
            entity.Property(e => e.Nombres).HasDefaultValue("");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
