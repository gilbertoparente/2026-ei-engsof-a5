using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public class RelatorioCategoriaPaisStrategy : IRelatorioStrategy
    {
        private readonly AppDbContext _context;

        public RelatorioCategoriaPaisStrategy(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RelatorioCategoriaPaisVM>> GerarRelatorio(
            string categoria,
            string pais,
            string skill)
        {
            var query = _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Include(t => t.Talentoskills)
                .ThenInclude(ts => ts.IdskillNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(t =>
                    t.IdcategoriaNavigation.Nome == categoria);
            }

            if (!string.IsNullOrEmpty(pais))
            {
                query = query.Where(t => t.Pais == pais);
            }

            if (!string.IsNullOrEmpty(skill))
            {
                query = query.Where(t =>
                    t.Talentoskills.Any(ts =>
                        ts.IdskillNavigation.Nome == skill));
            }

            var resultado = await query
                .GroupBy(t => new
                {
                    Categoria = t.IdcategoriaNavigation.Nome,
                    Pais = t.Pais
                })
                .Select(g => new RelatorioCategoriaPaisVM
                {
                    Categoria = g.Key.Categoria,
                    Pais = g.Key.Pais,
                    Total = g.Count(),
                    PrecoMedioMensal =
                        g.Average(t => t.Precohora * 176)
                })
                .ToListAsync();

            return resultado;
        }
    }
}