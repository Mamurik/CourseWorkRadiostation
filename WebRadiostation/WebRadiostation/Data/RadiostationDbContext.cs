using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebRadiostation.Models;

namespace WebRadiostation.Data;

public partial class RadiostationDbContext : DbContext
{
    public RadiostationDbContext()
    {
    }

    public RadiostationDbContext(DbContextOptions<RadiostationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<BroadcastSchedule> BroadcastSchedules { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<RecordDetail> RecordDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlServer("Server=HOME-PC;Database=RadiostationDb; Trusted_Connection =True; TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("PK__Artists__25706B5041F98A88");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasDefaultValue("Нет описания");
            entity.Property(e => e.Members)
                .HasMaxLength(255)
                .HasDefaultValue("Не указано");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BroadcastSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Broadcas__9C8A5B4942E32109");

            entity.ToTable("BroadcastSchedule");

            entity.Property(e => e.BroadcastDate).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.BroadcastSchedules)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Broadcast__Emplo__4E88ABD4");

            entity.HasOne(d => d.Record).WithMany(p => p.BroadcastSchedules)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Broadcast__Recor__4F7CD00D");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F1163AA13E2");

            entity.Property(e => e.Education)
                .HasMaxLength(100)
                .HasDefaultValue("Не указано");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasDefaultValue("Не указано");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385057E3234A048");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasDefaultValue("Нет описания");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Records__FBDF78E9E90EA28D");

            entity.Property(e => e.Album)
                .HasMaxLength(100)
                .HasDefaultValue("Не указано");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Artist).WithMany(p => p.Records)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Records__ArtistI__440B1D61");

            entity.HasOne(d => d.Genre).WithMany(p => p.Records)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Records__GenreId__44FF419A");
        });

        modelBuilder.Entity<RecordDetail>(entity =>
        {
            entity.HasKey(e => e.RecordDetailId).HasName("PK__RecordDe__33C10B79C016C13D");

            entity.HasIndex(e => e.RecordId, "UQ__RecordDe__FBDF78E8B5A96A49").IsUnique();

            entity.Property(e => e.RecordingDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Record).WithOne(p => p.RecordDetail)
                .HasForeignKey<RecordDetail>(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecordDet__Recor__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
