using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ninety.Models.Models;

public partial class NinetyContext : DbContext
{
    public NinetyContext()
    {
    }

    public NinetyContext(DbContextOptions<NinetyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BadmintonMatchDetail> BadmintonMatchDetails { get; set; }

    public virtual DbSet<Match> Matchs { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Ranking> Rankings { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamDetail> TeamDetails { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=Ninety;User Id=sa;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BadmintonMatchDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ApointSet1).HasColumnName("APointSet1");
            entity.Property(e => e.ApointSet2).HasColumnName("APointSet2");
            entity.Property(e => e.ApointSet3).HasColumnName("APointSet3");
            entity.Property(e => e.BpointSet1).HasColumnName("BPointSet1");
            entity.Property(e => e.BpointSet2).HasColumnName("BPointSet2");
            entity.Property(e => e.BpointSet3).HasColumnName("BPointSet3");

            entity.HasOne(d => d.Match).WithMany(p => p.BadmintonMatchDetails)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BadmintonMatchDetails_Matchs");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.TotalResult).HasMaxLength(50);

            entity.HasOne(d => d.Tournament).WithMany(p => p.Matches)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matchs_Tournaments");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 0)");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Users");
        });

        modelBuilder.Entity<Ranking>(entity =>
        {
            entity.ToTable("Ranking");

            entity.HasOne(d => d.Team).WithMany(p => p.Rankings)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ranking_Teams");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Rankings)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ranking_Tournaments");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Tournament).WithMany(p => p.Teams)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teams_Tournaments");
        });

        modelBuilder.Entity<TeamDetail>(entity =>
        {
            entity.HasOne(d => d.Team).WithMany(p => p.TeamDetails)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamDetails_Teams");

            entity.HasOne(d => d.User).WithMany(p => p.TeamDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamDetails_Users");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Format).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Place).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Sport).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tournaments_Sports");

            entity.HasOne(d => d.User).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tournaments_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(11);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
