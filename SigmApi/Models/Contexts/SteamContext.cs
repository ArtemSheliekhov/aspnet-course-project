using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SigmApi.Models;

namespace SigmApi.Models.Contexts;

public partial class SteamContext : DbContext
{
    public SteamContext()
    {
    }

    public SteamContext(DbContextOptions<SteamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyWorker> CompanyWorkers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2PD7NHH\\SQLEXPRESS;Initial Catalog=SteamData;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__2D971CAC06561B45");
        });

        modelBuilder.Entity<CompanyWorker>(entity =>
        {
            entity.HasKey(e => e.CompanyWorkerId).HasName("PK__CompanyW__A56662507C8BD44F");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyWorkers).HasConstraintName("FK__CompanyWo__Compa__4222D4EF");

            entity.HasOne(d => d.Worker).WithMany(p => p.CompanyWorkers).HasConstraintName("FK__CompanyWo__Worke__4316F928");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.AppId).HasName("PK__Game__8E2CF7F951726B4D");

            entity.HasOne(d => d.Company).WithMany(p => p.Games).HasConstraintName("FK__Game__CompanyId__3B75D760");

            entity.HasMany(d => d.Genres).WithMany(p => p.Apps)
                .UsingEntity<Dictionary<string, object>>(
                    "GameGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GameGenre__Genre__46E78A0C"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GameGenre__AppId__45F365D3"),
                    j =>
                    {
                        j.HasKey("AppId", "GenreId").HasName("PK__GameGenr__7E14A7AE62B79A44");
                        j.ToTable("GameGenre");
                    });

            entity.HasMany(d => d.Tags).WithMany(p => p.Apps)
                .UsingEntity<Dictionary<string, object>>(
                    "GameTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GameTag__TagId__4AB81AF0"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GameTag__AppId__49C3F6B7"),
                    j =>
                    {
                        j.HasKey("AppId", "TagId").HasName("PK__GameTag__587B3863C7B05A0F");
                        j.ToTable("GameTag");
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__0385057E0ABE7587");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tag__657CF9AC39EC4CB4");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("PK__Worker__077C8826690B46C2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
