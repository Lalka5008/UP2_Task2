using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Models;

namespace WpfApp2.Data;

public partial class Task4Context : DbContext
{
    public Task4Context()
    {
    }

    public Task4Context(DbContextOptions<Task4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<BidStatus> BidStatuses { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Сourse> Сourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Task4;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.BidId).HasName("PK__Bids__4A733D92B234DBB7");

            entity.Property(e => e.Comment)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.BidStatus).WithMany(p => p.Bids)
                .HasForeignKey(d => d.BidStatusId)
                .HasConstraintName("FK__Bids__BidStatusI__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Bids)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bids__UserId__5CD6CB2B");

            entity.HasOne(d => d.Сourse).WithMany(p => p.Bids)
                .HasForeignKey(d => d.СourseId)
                .HasConstraintName("FK__Bids__СourseId__5BE2A6F2");
        });

        modelBuilder.Entity<BidStatus>(entity =>
        {
            entity.HasKey(e => e.BidStatusId).HasName("PK__BidStatu__2EA94056CB976283");

            entity.ToTable("BidStatus");

            entity.Property(e => e.BidStatusName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__2D971CACE72681B1");

            entity.ToTable("Company");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Direction>(entity =>
        {
            entity.HasKey(e => e.DirectionId).HasName("PK__Directio__876847C6265C2F05");

            entity.ToTable("Direction");

            entity.Property(e => e.DirectionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Discrption)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A824B54D2");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF259646EEED6C6");

            entity.Property(e => e.Discription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeacherType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C73808650");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Users__CompanyId__4F7CD00D");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleId__4E88ABD4");
        });

        modelBuilder.Entity<Сourse>(entity =>
        {
            entity.HasKey(e => e.СourseId).HasName("PK__Сourses__CBD3AC418D21508C");

            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.СourseName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Direction).WithMany(p => p.Сourses)
                .HasForeignKey(d => d.DirectionId)
                .HasConstraintName("FK__Сourses__Directi__5629CD9C");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Сourses)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Сourses__Teacher__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
