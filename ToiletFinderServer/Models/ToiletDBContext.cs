﻿using System;
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

    public virtual DbSet<CurrentToiletsPhoto> CurrentToiletsPhotos { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Utype> Utypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=ToiletFinder_DB;User ID=TaskAdminLogin;Password=ShaharAdmin;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrentToilet>(entity =>
        {
            entity.HasKey(e => e.ToiletId).HasName("PK__CurrentT__A922EA2D693281D9");

            entity.HasOne(d => d.Status).WithMany(p => p.CurrentToilets).HasConstraintName("FK__CurrentTo__Statu__2C3393D0");

            entity.HasOne(d => d.User).WithMany(p => p.CurrentToilets).HasConstraintName("FK__CurrentTo__UserI__2D27B809");
        });

        modelBuilder.Entity<CurrentToiletsPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__CurrentT__21B7B5E2AD609264");

            entity.HasOne(d => d.Toilet).WithMany(p => p.CurrentToiletsPhotos).HasConstraintName("FK__CurrentTo__Toile__300424B4");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.ToiletId).HasName("PK__Rates__A922EA0D3CF59E1C");

            entity.Property(e => e.ToiletId).ValueGeneratedNever();

            entity.HasOne(d => d.Toilet).WithOne(p => p.Rate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rates__ToiletID__32E0915F");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ToiletId).HasName("PK__Reviews__A922EA0D8A822F12");

            entity.Property(e => e.ToiletId).ValueGeneratedNever();

            entity.HasOne(d => d.Toilet).WithOne(p => p.Review)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__ToiletI__35BCFE0A");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Statuses__C8EE204336337FDA");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C76E22F5F");

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.Users).HasConstraintName("FK__Users__UserType__29572725");
        });

        modelBuilder.Entity<Utype>(entity =>
        {
            entity.HasKey(e => e.UserType).HasName("PK__UTypes__87E786904F61F1CB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
