using System;
using Microsoft.EntityFrameworkCore;

namespace ProfileMAnager.Models;

public partial class GerirProposta : DbContext
{
    public GerirProposta() { }

    public GerirProposta(DbContextOptions<GerirProposta> options) : base(options) { }

    public virtual DbSet<PropostaTalento> PropostaTalentos { get; set; }
    public virtual DbSet<Categoriatalento> Categoriatalentos { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Propostaskill> Propostaskills { get; set; }
    public virtual DbSet<Propostatrabalho> Propostatrabalhos { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }
    public virtual DbSet<Talento> Talentos { get; set; }
    public virtual DbSet<Talentoskill> Talentoskills { get; set; }
    public virtual DbSet<Utilizador> Utilizadors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ToLower());
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToLower());
            }
        }

        modelBuilder.Entity<PropostaTalento>(entity =>
        {
            // propostatalento
            entity.ToTable("propostatalento");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Datainicio).HasColumnName("datainicio");
            entity.Property(e => e.Datafim).HasColumnName("datafim");

            entity.HasOne(d => d.Talento)
                .WithMany(p => p.PropostaTalentos)
                .HasForeignKey(d => d.Idtalento);

            entity.HasOne(d => d.Proposta)
                .WithMany(p => p.PropostaTalentos)
                .HasForeignKey(d => d.Idproposta);
        });

        // Talentoskill
        modelBuilder.Entity<Talentoskill>(entity =>
        {
            entity.HasKey(e => new { e.Idtalento, e.Idskill });
            entity.ToTable("talentoskill");

            entity.HasOne(d => d.IdskillNavigation)
                .WithMany(p => p.Talentoskills)
                .HasForeignKey(d => d.Idskill);

            entity.HasOne(d => d.IdtalentoNavigation)
                .WithMany(p => p.Talentoskills)
                .HasForeignKey(d => d.Idtalento);
        });

        // Propostaskill
        modelBuilder.Entity<Propostaskill>(entity =>
        {
            entity.HasKey(e => new { e.Idproposta, e.Idskill });
            entity.ToTable("propostaskill");

            entity.HasOne(d => d.IdpropostaNavigation)
                .WithMany(p => p.Propostaskills)
                .HasForeignKey(d => d.Idproposta);

            entity.HasOne(d => d.IdskillNavigation)
                .WithMany(p => p.Propostaskills)
                .HasForeignKey(d => d.Idskill);
        });

        // Propostatrabalho
        modelBuilder.Entity<Propostatrabalho>(entity =>
        {
            entity.HasKey(e => e.Idproposta);
            entity.ToTable("propostatrabalho");

            entity.HasOne(d => d.IdclienteNavigation)
                .WithMany(p => p.Propostatrabalhos)
                .HasForeignKey(d => d.Idcliente);

            entity.HasOne(d => d.IdcategoriaNavigation)
                .WithMany(p => p.Propostatrabalhos)
                .HasForeignKey(d => d.Idcategoria);
        });

        // Tabela
        modelBuilder.Entity<Cliente>(entity => { entity.HasKey(e => e.Idcliente); entity.ToTable("cliente"); });
        modelBuilder.Entity<Talento>(entity => { entity.HasKey(e => e.Idtalento); entity.ToTable("talento"); });
        modelBuilder.Entity<Skill>(entity => { entity.HasKey(e => e.Idskill); entity.ToTable("skill"); });
        modelBuilder.Entity<Categoriatalento>(entity => { entity.HasKey(e => e.Idcategoria); entity.ToTable("categoriatalento"); });
        modelBuilder.Entity<Utilizador>(entity => { entity.HasKey(e => e.Idutilizador); entity.ToTable("utilizador"); });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}