using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public class TalentoService : ITalentoService
    {
        private readonly AppDbContext _context;

        public TalentoService(AppDbContext context)
        {
            _context = context;
        }

        // 1. Obter talentos de um utilizador específico
        public async Task<IEnumerable<Talento>> GetTalentosPorUtilizadorAsync(int userId)
        {
            return await _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Where(t => t.Idutilizador == userId)
                .ToListAsync();
        }

        // 2. Obter detalhes completos (Skills, Experiência, Categoria)
        public async Task<Talento?> GetDetalhesCompletosAsync(int id)
        {
            return await _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Include(t => t.Talentoskills).ThenInclude(ts => ts.IdskillNavigation)
                .Include(t => t.Experiencia)
                .FirstOrDefaultAsync(m => m.Idtalento == id);
        }

        // 3. Criar um novo talento
        public async Task<int> CriarTalentoAsync(Talento talento, int userId)
        {
            talento.Idutilizador = userId;
            talento.CreatedAt = DateTime.UtcNow;
            talento.UpdatedAt = DateTime.UtcNow;

            _context.Add(talento);
            await _context.SaveChangesAsync();
            
            return talento.Idtalento;
        }

        // get 
        public async Task<string?> AdicionarSkillAsync(Talentoskill ts)
        {
            var existe = await _context.Talentoskills
                .AnyAsync(x => x.Idtalento == ts.Idtalento && x.Idskill == ts.Idskill);

            if (existe)
                return "Este talento já possui esta skill associada.";

            _context.Talentoskills.Add(ts);
            await _context.SaveChangesAsync();
            return null; 
        }
        
        public async Task<string?> AdicionarExperienciaAsync(Experiencia exp)
        {
            if (exp.Anofim.HasValue && exp.Anofim < exp.Anoinicio)
                return "O ano de término não pode ser anterior ao início.";

            var experienciasExistentes = await _context.Experiencia
                .Where(e => e.Idtalento == exp.Idtalento)
                .ToListAsync();

            int fimNovo = exp.Anofim ?? DateTime.Now.Year;

            foreach (var e in experienciasExistentes)
            {
                int fimExistente = e.Anofim ?? DateTime.Now.Year;
                
                bool sobrepoe = (exp.Anoinicio >= e.Anoinicio && exp.Anoinicio <= fimExistente) ||
                                (fimNovo >= e.Anoinicio && fimNovo <= fimExistente) ||
                                (e.Anoinicio >= exp.Anoinicio && e.Anoinicio <= fimNovo);

                if (sobrepoe)
                    return $"Sobreposição detetada com a experiência em '{e.Empresa}'.";
            }

            exp.CreatedAt = DateTime.UtcNow;
            exp.UpdatedAt = DateTime.UtcNow;

            _context.Experiencia.Add(exp);
            await _context.SaveChangesAsync();
            return null;
        }
        
        public async Task AtualizarTalentoAsync(Talento talento)
        {
            _context.Update(talento);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarTalentoAsync(int id)
        {
            var talento = await _context.Talentos
                .Include(t => t.Talentoskills)
                .Include(t => t.Experiencia)
                .FirstOrDefaultAsync(t => t.Idtalento == id);

            if (talento != null)
            {
                
                _context.Talentoskills.RemoveRange(talento.Talentoskills);
                _context.Experiencia.RemoveRange(talento.Experiencia);
                _context.Talentos.Remove(talento);
                await _context.SaveChangesAsync();
            }
        }
    }
}