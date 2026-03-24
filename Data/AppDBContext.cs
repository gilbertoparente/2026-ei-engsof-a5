using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Models;

namespace ProfileMAnager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("estadoproposta", new[] { "ABERTA", "EM_PROGRESSO", "FECHADA" });

            modelBuilder.Entity<Areaprofissional>(entity =>
            {
                entity.HasKey(e => e.Idarea).HasName("areaprofissional_pkey");
                entity.ToTable("areaprofissional");
                entity.HasIndex(e => e.Nome, "areaprofissional_nome_key").IsUnique();
            });

            modelBuilder.Entity<Categoriatalento>(entity =>
            {
                entity.HasKey(e => e.Idcategoria).HasName("categoriatalento_pkey");
                entity.ToTable("categoriatalento");
                entity.HasIndex(e => e.Nome, "categoriatalento_nome_key").IsUnique();
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Idcliente).HasName("cliente_pkey");
                entity.ToTable("cliente");
                entity.HasOne(d => d.IdutilizadorNavigation).WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.Idutilizador)
                    .HasConstraintName("fk_cliente_utilizador");
            });

            modelBuilder.Entity<Experiencia>(entity =>
            {
                entity.HasKey(e => e.Idexperiencia).HasName("experiencia_pkey");
                entity.ToTable("experiencia");
                entity.HasOne(d => d.IdtalentoNavigation).WithMany(p => p.Experiencia)
                    .HasForeignKey(d => d.Idtalento)
                    .HasConstraintName("fk_experiencia_talento");
            });

            modelBuilder.Entity<Propostaskill>(entity =>
            {
                entity.HasKey(e => new { e.Idproposta, e.Idskill }).HasName("propostaskill_pkey");
                entity.ToTable("propostaskill");
                entity.HasOne(d => d.IdpropostaNavigation).WithMany(p => p.Propostaskills)
                    .HasForeignKey(d => d.Idproposta);
                entity.HasOne(d => d.IdskillNavigation).WithMany(p => p.Propostaskills)
                    .HasForeignKey(d => d.Idskill);
            });

            modelBuilder.Entity<Propostatrabalho>(entity =>
            {
                entity.HasKey(e => e.Idproposta).HasName("propostatrabalho_pkey");
                entity.ToTable("propostatrabalho");
                entity.Property(e => e.Estado).HasColumnType("estadoproposta");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Idskill).HasName("skill_pkey");
                entity.ToTable("skill");
                entity.HasOne(d => d.IdareaNavigation).WithMany(p => p.Skills).HasForeignKey(d => d.Idarea);
            });

            // CONFIGURAÇÃO DO TALENTO
            modelBuilder.Entity<Talento>(entity =>
            {
                entity.HasKey(e => e.Idtalento).HasName("talento_pkey");
                entity.ToTable("talento");

                entity.HasOne(d => d.IdcategoriaNavigation)
                    .WithMany(p => p.Talentos)
                    .HasForeignKey(d => d.Idcategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_talento_categoria");

                entity.HasOne(d => d.IdutilizadorNavigation)
                    .WithMany(p => p.Talentos)
                    .HasForeignKey(d => d.Idutilizador)
                    .HasConstraintName("fk_talento_utilizador");
            }); // FECHA O BLOCO DO TALENTO

            // CONFIGURAÇÃO DO TALENTOSKILL
            modelBuilder.Entity<Talentoskill>(entity =>
            {
                // CHAVE COMPOSTA PARA A TABELA DE LIGAÇÃO
                entity.HasKey(e => new { e.Idtalento, e.Idskill }).HasName("talentoskill_pkey");
                entity.ToTable("talentoskill");

                entity.HasOne(d => d.IdtalentoNavigation)
                    .WithMany(p => p.Talentoskills)
                    .HasForeignKey(d => d.Idtalento)
                    .HasConstraintName("fk_talentoskill_talento");

                entity.HasOne(d => d.IdskillNavigation)
                    .WithMany(p => p.Talentoskills)
                    .HasForeignKey(d => d.Idskill)
                    .HasConstraintName("fk_talentoskill_skill");
            }); // FECHA O BLOCO DO TALENTOSKILL

            modelBuilder.Entity<Utilizador>(entity =>
            {
                entity.HasKey(e => e.Idutilizador).HasName("utilizador_pkey");
                entity.ToTable("utilizador");
                entity.HasIndex(e => e.Email, "utilizador_email_key").IsUnique();
            });

            // --- SOLUÇÃO GLOBAL PARA SNAKE_CASE (PostgreSQL padrão) ---
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Converter Nome da Tabela: Exemplo "Areaprofissional" -> "areaprofissional"
                entity.SetTableName(entity.GetTableName().ToLower());

                foreach (var property in entity.GetProperties())
                {
                    // Converter Propriedades: Exemplo "CreatedAt" -> "created_at"
                    var columnName = property.GetColumnName();
        
                    // Esta lógica insere um '_' antes de cada letra maiúscula (exceto a primeira)
                    // e transforma tudo em minúsculas.
                    var snakeCaseName = System.Text.RegularExpressions.Regex
                        .Replace(property.Name, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
            
                    property.SetColumnName(snakeCaseName);
                }

                // Fazer o mesmo para chaves e índices se necessário
                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName().ToLower());

                foreach (var key in entity.GetForeignKeys())
                    key.SetConstraintName(key.GetConstraintName().ToLower());

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(index.GetDatabaseName().ToLower());
            }
        }
    }
}