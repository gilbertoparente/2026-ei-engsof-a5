using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public class RelatorioService
    {
        private readonly AppDbContext _context;

        public RelatorioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RelatorioCategoriaPaisVM>> GetRelatorioCategoriaPais(
            string categoria,
            string pais,
            string skill)
        {
            var query = _context.Talentoskills
                .Include(ts => ts.IdtalentoNavigation)
                    .ThenInclude(t => t.IdcategoriaNavigation)
                .Include(ts => ts.IdskillNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(ts =>
                    ts.IdtalentoNavigation.IdcategoriaNavigation.Nome == categoria);
            }

            if (!string.IsNullOrEmpty(pais))
            {
                query = query.Where(ts =>
                    ts.IdtalentoNavigation.Pais == pais);
            }

            if (!string.IsNullOrEmpty(skill))
            {
                query = query.Where(ts =>
                    ts.IdskillNavigation.Nome == skill);
            }

            return await query
                .GroupBy(ts => new
                {
                    Categoria = ts.IdtalentoNavigation.IdcategoriaNavigation.Nome,
                    Pais = ts.IdtalentoNavigation.Pais,
                    Skill = ts.IdskillNavigation.Nome
                })
                .Select(g => new RelatorioCategoriaPaisVM
                {
                    Categoria = g.Key.Categoria,
                    Pais = g.Key.Pais,
                    Skill = g.Key.Skill,
                    Total = g.Select(x => x.IdtalentoNavigation.Idtalento)
                             .Distinct()
                             .Count(),
                    PrecoMedioMensal =
                        g.Average(x => x.IdtalentoNavigation.Precohora) * 176
                })
                .OrderBy(x => x.Categoria)
                .ThenBy(x => x.Pais)
                .ThenBy(x => x.Skill)
                .ToListAsync();
        }
    }
}