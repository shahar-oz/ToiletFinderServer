using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class ToiletDBContext : DbContext
{
    public ToiletDBContext()
    {
    }

    public ToiletDBContext(DbContextOptions<ToiletDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CurrentToilet> CurrentToilets { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Utype> Utypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=ToiletFinder_DB;User ID=TaskAdminLogin;Password=ShaharAdmin;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrentToilet>(entity =>
        {
            entity.HasKey(e => e.ToiletId).HasName("PK__CurrentT__A922EA2D2F282332");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.ToiletId).HasName("PK__Rates__A922EA0D9D93F948");

            entity.Property(e => e.ToiletId).ValueGeneratedNever();

            entity.HasOne(d => d.Toilet).WithOne(p => p.Rate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rates__ToiletID__2B3F6F97");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ToiletId).HasName("PK__Reviews__A922EA0D31C6EE3D");

            entity.Property(e => e.ToiletId).ValueGeneratedNever();

            entity.HasOne(d => d.Toilet).WithOne(p => p.Review)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__ToiletI__2E1BDC42");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CD593A325");

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.Users).HasConstraintName("FK__Users__UserType__267ABA7A");
        });

        modelBuilder.Entity<Utype>(entity =>
        {
            entity.HasKey(e => e.UserType).HasName("PK__UTypes__87E786907A54AFAC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
