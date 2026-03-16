using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProfileMAnager.Models;

public partial class GerirProposta : DbContext
{
    public GerirProposta()
    {
    }

    public GerirProposta(DbContextOptions<GerirProposta> options)
        : base(options)
    {
    }

    public virtual DbSet<Areaprofissional> Areaprofissionals { get; set; }

    public virtual DbSet<Categoriatalento> Categoriatalentos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Experiencia> Experiencia { get; set; }

    public virtual DbSet<Propostaskill> Propostaskills { get; set; }

    public virtual DbSet<Propostatrabalho> Propostatrabalhos { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Talento> Talentos { get; set; }

    public virtual DbSet<Talentoskill> Talentoskills { get; set; }

    public virtual DbSet<Utilizador> Utilizadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ProjectManager;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("estadoproposta", new[] { "ABERTA", "EM_PROGRESSO", "FECHADA" });

        modelBuilder.Entity<Areaprofissional>(entity =>
        {
            entity.HasKey(e => e.Idarea).HasName("areaprofissional_pkey");

            entity.ToTable("areaprofissional");

            entity.HasIndex(e => e.Nome, "areaprofissional_nome_key").IsUnique();

            entity.Property(e => e.Idarea).HasColumnName("idarea");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
        });
        
        

        modelBuilder.Entity<Categoriatalento>(entity =>
        {
            entity.HasKey(e => e.Idcategoria).HasName("categoriatalento_pkey");

            entity.ToTable("categoriatalento");

            entity.HasIndex(e => e.Nome, "categoriatalento_nome_key").IsUnique();

            entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Idcliente).HasName("cliente_pkey");

            entity.ToTable("cliente");

            entity.Property(e => e.Idcliente).HasColumnName("idcliente");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Idutilizador).HasColumnName("idutilizador");
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .HasColumnName("nome");
            entity.Property(e => e.Pais)
                .HasMaxLength(100)
                .HasColumnName("pais");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdutilizadorNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.Idutilizador)
                .HasConstraintName("fk_cliente_utilizador");
        });

        modelBuilder.Entity<Experiencia>(entity =>
        {
            entity.HasKey(e => e.Idexperiencia).HasName("experiencia_pkey");

            entity.ToTable("experiencia");

            entity.HasIndex(e => new { e.Idtalento, e.Empresa, e.Anoinicio }, "unique_experiencia").IsUnique();

            entity.Property(e => e.Idexperiencia).HasColumnName("idexperiencia");
            entity.Property(e => e.Anofim).HasColumnName("anofim");
            entity.Property(e => e.Anoinicio).HasColumnName("anoinicio");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Empresa)
                .HasMaxLength(150)
                .HasColumnName("empresa");
            entity.Property(e => e.Idtalento).HasColumnName("idtalento");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdtalentoNavigation).WithMany(p => p.Experiencia)
                .HasForeignKey(d => d.Idtalento)
                .HasConstraintName("fk_experiencia_talento");
        });

        modelBuilder.Entity<Propostaskill>(entity =>
        {
            entity.HasKey(e => new { e.Idproposta, e.Idskill }).HasName("propostaskill_pkey");

            entity.ToTable("propostaskill");

            entity.HasIndex(e => e.Idskill, "idx_propostaskill_skill");

            entity.Property(e => e.Idproposta).HasColumnName("idproposta");
            entity.Property(e => e.Idskill).HasColumnName("idskill");
            entity.Property(e => e.Anosminimosexperiencia).HasColumnName("anosminimosexperiencia");

            entity.HasOne(d => d.IdpropostaNavigation).WithMany(p => p.Propostaskills)
                .HasForeignKey(d => d.Idproposta)
                .HasConstraintName("fk_propostaskill_proposta");

            entity.HasOne(d => d.IdskillNavigation).WithMany(p => p.Propostaskills)
                .HasForeignKey(d => d.Idskill)
                .HasConstraintName("fk_propostaskill_skill");
        });

        modelBuilder.Entity<Propostatrabalho>(entity =>
        
        {
            entity.HasKey(e => e.Idproposta).HasName("propostatrabalho_pkey");

            entity.ToTable("propostatrabalho");

            entity.HasIndex(e => e.Idcategoria, "idx_proposta_categoria");

            entity.Property(e => e.Idproposta).HasColumnName("idproposta");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Horastotais).HasColumnName("horastotais");
            entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");
            entity.Property(e => e.Idcliente).HasColumnName("idcliente");
            entity.Property(e => e.Idutilizador).HasColumnName("idutilizador");
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .HasColumnName("nome");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdcategoriaNavigation).WithMany(p => p.Propostatrabalhos)
                .HasForeignKey(d => d.Idcategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_proposta_categoria");

            entity.HasOne(d => d.IdclienteNavigation).WithMany(p => p.Propostatrabalhos)
                .HasForeignKey(d => d.Idcliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_proposta_cliente");

            entity.HasOne(d => d.IdutilizadorNavigation).WithMany(p => p.Propostatrabalhos)
                .HasForeignKey(d => d.Idutilizador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_proposta_utilizador");
            
            entity.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasColumnType("estadoproposta");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Idskill).HasName("skill_pkey");

            entity.ToTable("skill");

            entity.HasIndex(e => e.Nome, "skill_nome_key").IsUnique();

            entity.Property(e => e.Idskill).HasColumnName("idskill");
            entity.Property(e => e.Idarea).HasColumnName("idarea");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdareaNavigation).WithMany(p => p.Skills)
                .HasForeignKey(d => d.Idarea)
                .HasConstraintName("skill_idarea_fkey");
        });

        modelBuilder.Entity<Talento>(entity =>
        {
            entity.HasKey(e => e.Idtalento).HasName("talento_pkey");

            entity.ToTable("talento");

            entity.HasIndex(e => e.Idcategoria, "idx_talento_categoria");

            entity.HasIndex(e => e.pais, "idx_talento_pais");

            entity.Property(e => e.Idtalento).HasColumnName("idtalento");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");
            entity.Property(e => e.Idutilizador).HasColumnName("idutilizador");
            entity.Property(e => e.pais)
                .HasMaxLength(100)
                .HasColumnName("pais");
            entity.Property(e => e.precohora)
                .HasPrecision(10, 2)
                .HasColumnName("precohora");
            entity.Property(e => e.Publico)
                .HasDefaultValueSql("true")
                .HasColumnName("publico");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdcategoriaNavigation).WithMany(p => p.Talentos)
                .HasForeignKey(d => d.Idcategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_talento_categoria");

            entity.HasOne(d => d.IdutilizadorNavigation).WithMany(p => p.Talentos)
                .HasForeignKey(d => d.Idutilizador)
                .HasConstraintName("fk_talento_utilizador");
        });

        modelBuilder.Entity<Talentoskill>(entity =>
        {
            entity.HasKey(e => new { e.Idtalento, e.Idskill }).HasName("talentoskill_pkey");

            entity.ToTable("talentoskill");

            entity.HasIndex(e => e.Idskill, "idx_talentoskill_skill");

            entity.Property(e => e.Idtalento).HasColumnName("idtalento");
            entity.Property(e => e.Idskill).HasColumnName("idskill");
            entity.Property(e => e.Anosexperiencia).HasColumnName("anosexperiencia");

            entity.HasOne(d => d.IdskillNavigation).WithMany(p => p.Talentoskills)
                .HasForeignKey(d => d.Idskill)
                .HasConstraintName("fk_talentoskill_skill");

            entity.HasOne(d => d.IdtalentoNavigation).WithMany(p => p.Talentoskills)
                .HasForeignKey(d => d.Idtalento)
                .HasConstraintName("fk_talentoskill_talento");
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.Idutilizador).HasName("utilizador_pkey");

            entity.ToTable("utilizador");

            entity.HasIndex(e => e.Email, "utilizador_email_key").IsUnique();

            entity.Property(e => e.Idutilizador).HasColumnName("idutilizador");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .HasColumnName("nome");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
