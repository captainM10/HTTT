using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyPhamCheilinus.Models;

public partial class CanhGacContext : DbContext
{
    public CanhGacContext()
    {
    }

    public CanhGacContext(DbContextOptions<CanhGacContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<CaGac> CaGacs { get; set; }

    public virtual DbSet<CapBac> CapBacs { get; set; }

    public virtual DbSet<ChucVu> ChucVus { get; set; }

    public virtual DbSet<DonVi> DonVis { get; set; }

    public virtual DbSet<HocVien> HocViens { get; set; }

    public virtual DbSet<NhiemVu> NhiemVus { get; set; }

    public virtual DbSet<Pcgac> Pcgacs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ThongTinGac> ThongTinGacs { get; set; }

    public virtual DbSet<VongGac> VongGacs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HAHAHAPHUONG\\MSSQLSERVER01;Initial Catalog=Canh_gac;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.LastLogin).HasColumnType("date");
            entity.Property(e => e.MaDonVi)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Pasword).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Roles");
        });

        modelBuilder.Entity<CaGac>(entity =>
        {
            entity.HasKey(e => e.MaCaGac);

            entity.ToTable("CaGac");

            entity.Property(e => e.MaCaGac).ValueGeneratedNever();
        });

        modelBuilder.Entity<CapBac>(entity =>
        {
            entity.HasKey(e => e.MaCapBac).HasName("PK__CapBac__2190882526F3CB1B");

            entity.ToTable("CapBac");

            entity.Property(e => e.MaCapBac)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TenCapBac).HasMaxLength(50);
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.MaChucVu).HasName("PK__ChucVu__D463953373C381DF");

            entity.ToTable("ChucVu");

            entity.Property(e => e.MaChucVu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TenChucVu).HasMaxLength(50);
        });

        modelBuilder.Entity<DonVi>(entity =>
        {
            entity.HasKey(e => e.MaDonVi).HasName("PK_DaiDoi");

            entity.ToTable("DonVi");

            entity.Property(e => e.MaDonVi)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MauSac).HasMaxLength(50);
            entity.Property(e => e.TenDonVi).HasMaxLength(50);
        });

        modelBuilder.Entity<HocVien>(entity =>
        {
            entity.HasKey(e => e.MaHocVien).HasName("PK__HocVien__685B0E6A05DA6EB9");

            entity.ToTable("HocVien");

            entity.Property(e => e.MaHocVien)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GioiTinh).HasMaxLength(4);
            entity.Property(e => e.MaCapBac)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaChucVu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaDaiDoi)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.TenHocVien).HasMaxLength(150);

            entity.HasOne(d => d.MaCapBacNavigation).WithMany(p => p.HocViens)
                .HasForeignKey(d => d.MaCapBac)
                .HasConstraintName("FK_HocVien_CapBac");

            entity.HasOne(d => d.MaChucVuNavigation).WithMany(p => p.HocViens)
                .HasForeignKey(d => d.MaChucVu)
                .HasConstraintName("FK_HocVien_ChucVu");

            entity.HasOne(d => d.MaDaiDoiNavigation).WithMany(p => p.HocViens)
                .HasForeignKey(d => d.MaDaiDoi)
                .HasConstraintName("FK_HocVien_DonVi");
        });

        modelBuilder.Entity<NhiemVu>(entity =>
        {
            entity.HasKey(e => e.MaNhiemVu);

            entity.ToTable("NhiemVu");

            entity.Property(e => e.TenNhiemVu).HasMaxLength(50);

            entity.HasOne(d => d.MaVongGacNavigation).WithMany(p => p.NhiemVus)
                .HasForeignKey(d => d.MaVongGac)
                .HasConstraintName("FK_NhiemVu_VongGac");
        });

        modelBuilder.Entity<Pcgac>(entity =>
        {
            entity.HasKey(e => new { e.Ngay, e.MaHocVien });

            entity.ToTable("PCGac", tb => tb.HasTrigger("TR_PCGac_AfterInsertUpdate"));

            entity.Property(e => e.Ngay).HasColumnType("date");
            entity.Property(e => e.MaHocVien)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaCaGacNavigation).WithMany(p => p.Pcgacs)
                .HasForeignKey(d => d.MaCaGac)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PCGac_CaGac");

            entity.HasOne(d => d.MaHocVienNavigation).WithMany(p => p.Pcgacs)
                .HasForeignKey(d => d.MaHocVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PCGac_HocVien");

            entity.HasOne(d => d.MaNhiemVuNavigation).WithMany(p => p.Pcgacs)
                .HasForeignKey(d => d.MaNhiemVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PCGac_NhiemVu");

            entity.HasOne(d => d.NgayNavigation).WithMany(p => p.Pcgacs)
                .HasForeignKey(d => d.Ngay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PCGac_ThongTinGac");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<ThongTinGac>(entity =>
        {
            entity.HasKey(e => e.Ngay);

            entity.ToTable("ThongTinGac");

            entity.Property(e => e.Ngay).HasColumnType("date");
            entity.Property(e => e.Dap).HasMaxLength(50);
            entity.Property(e => e.Hoi).HasMaxLength(50);
            entity.Property(e => e.MaDonVi)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MaDonViNavigation).WithMany(p => p.ThongTinGacs)
                .HasForeignKey(d => d.MaDonVi)
                .HasConstraintName("FK_ThongTinGac_DonVi");
        });

        modelBuilder.Entity<VongGac>(entity =>
        {
            entity.HasKey(e => e.MaVongGac);

            entity.ToTable("VongGac");

            entity.Property(e => e.TenVongGac).HasMaxLength(50);
            entity.Property(e => e.ViTri).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
