using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CarnetDigital.DataAccess.Models;

public partial class Tiusr13plCarnetDigitalContext : DbContext
{
    public Tiusr13plCarnetDigitalContext()
    {
    }

    public Tiusr13plCarnetDigitalContext(DbContextOptions<Tiusr13plCarnetDigitalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Carrera> Carreras { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<TelefonoUsuario> TelefonoUsuarios { get; set; }

    public virtual DbSet<TipoIdentificacion> TipoIdentificacions { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tiusr13pl.cuc-carrera-ti.ac.cr\\mssqlserver2019;Database=tiusr13pl_CarnetDigital;User=cd_softcode;Password=Y12&7m5dk;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("cd_softcode");

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Areas__70B82028D444520C");

            entity.ToTable("Area", "dbo");

            entity.Property(e => e.AreaId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AreaID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.CarreraId).HasName("PK__Carreras__3E43B181B4F79E28");

            entity.ToTable("Carrera", "dbo");

            entity.Property(e => e.CarreraId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CarreraID");
            entity.Property(e => e.DirectorCarrera)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreCarrera)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estados", "dbo");

            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken", "dbo");

            entity.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.RefreshTokenValue)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshToken_Usuarios");
        });

        modelBuilder.Entity<TelefonoUsuario>(entity =>
        {
            entity.HasKey(e => new { e.Email, e.Telefono }).HasName("PK_TelefonosUsuario");

            entity.ToTable("TelefonoUsuario", "dbo");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.TelefonoUsuarios)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelefonosUsuario_Usuarios");
        });

        modelBuilder.Entity<TipoIdentificacion>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__TiposIde__C774CA54DDC07D3B");

            entity.ToTable("TipoIdentificacion", "dbo");

            entity.Property(e => e.Email).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.TipoUsuarioId).HasName("PK__TiposUsu__7F22C7028463AF5F");

            entity.ToTable("TipoUsuario", "dbo");

            entity.Property(e => e.TipoUsuarioId)
                .ValueGeneratedOnAdd()
                .HasColumnName("TipoUsuarioID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Usuarios__A9D105356EEA241C");

            entity.ToTable("Usuario", "dbo");

            entity.HasIndex(e => e.Identificacion, "IX_Usuarios").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Fiotografia).IsUnicode(false);
            entity.Property(e => e.Identificacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TipoIdentificacionId).HasColumnName("TipoIdentificacionID");
            entity.Property(e => e.TipoUsuarioId).HasColumnName("TipoUsuarioID");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Estado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Estados");

            entity.HasOne(d => d.TipoIdentificacion).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.TipoIdentificacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__TipoUs__3B75D760");

            entity.HasOne(d => d.TipoUsuario).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.TipoUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__TipoUs__3C69FB99");

            entity.HasMany(d => d.Areas).WithMany(p => p.Emails)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioArea",
                    r => r.HasOne<Area>().WithMany()
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioAr__AreaI__47DBAE45"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioAr__Usuar__46E78A0C"),
                    j =>
                    {
                        j.HasKey("Email", "AreaId").HasName("PK__UsuarioA__41F2B3E643365044");
                        j.ToTable("UsuarioArea", "dbo");
                        j.IndexerProperty<string>("Email")
                            .HasMaxLength(100)
                            .IsUnicode(false);
                        j.IndexerProperty<byte>("AreaId").HasColumnName("AreaID");
                    });

            entity.HasMany(d => d.Carreras).WithMany(p => p.Emails)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioCarrera",
                    r => r.HasOne<Carrera>().WithMany()
                        .HasForeignKey("CarreraId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioCa__Carre__4222D4EF"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioCa__Usuar__412EB0B6"),
                    j =>
                    {
                        j.HasKey("Email", "CarreraId").HasName("PK__UsuarioC__D0FCCBBA9595A5F8");
                        j.ToTable("UsuarioCarrera", "dbo");
                        j.IndexerProperty<string>("Email")
                            .HasMaxLength(100)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("CarreraId")
                            .HasMaxLength(5)
                            .IsUnicode(false)
                            .HasColumnName("CarreraID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
