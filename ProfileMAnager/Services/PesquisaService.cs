using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public class PesquisaService : IPesquisaService
    {
        private readonly AppDbContext _context;
        public PesquisaService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Skill>> GetListaSkillsOrdenadaAsync()
        {
            return await _context.Skills.OrderBy(s => s.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Talento>> PesquisarTalentosPorSkillsAsync(int[] skillIds)
        {
            if (skillIds == null || skillIds.Length == 0)
                return new List<Talento>();

            var query = _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Where(t => t.Publico == true);
            
            foreach (var id in skillIds)
            {
                query = query.Where(t => t.Talentoskills.Any(ts => ts.Idskill == id));
            }

            return await query.OrderBy(t => t.Nome).ToListAsync();
        }
    }
}