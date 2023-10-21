using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrdersDashboard.Models;

namespace OrdersDashboard.Context;

public partial class ContractorsOrdersContext : DbContext
{
    public ContractorsOrdersContext()
    {
    }

    public ContractorsOrdersContext(DbContextOptions<ContractorsOrdersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contractor> Contractors { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-IONTORB\\SQLEXPRESS;Database=CONTRACTORS_ORDERS;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contractor>(entity =>
        {
            entity.HasKey(e => e.IdKontrahenta).HasName("PK_KONTRAHENCI");

            entity.ToTable("Contractor");

            entity.Property(e => e.IdKontrahenta).HasColumnName("ID_KONTRAHENTA");
            entity.Property(e => e.DataDodania).HasColumnName("DATA_DODANIA");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAZWA");
            entity.Property(e => e.Nip)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NIP");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdZamowienia).HasName("PK_ZAMOWIENIA");

            entity.ToTable("Order");

            entity.Property(e => e.IdZamowienia).HasColumnName("ID_ZAMOWIENIA");
            entity.Property(e => e.DataDodania).HasColumnName("DATA_DODANIA");
            entity.Property(e => e.IdKontrahenta).HasColumnName("ID_KONTRAHENTA");
            entity.Property(e => e.Numer)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NUMER");

            entity.HasOne(d => d.IdKontrahentaNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdKontrahenta)
                .HasConstraintName("FK_KONTRAHENT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
