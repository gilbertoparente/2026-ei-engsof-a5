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

        public async Task<List<RelatorioCategoriaPaisVM>> GetRelatorioCategoriaPais(string categoria, string pais)
        {
            var query = _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(categoria))
                query = query.Where(t => t.IdcategoriaNavigation.Nome == categoria);

            if (!string.IsNullOrEmpty(pais))
                query = query.Where(t => t.Pais == pais);

            return await query
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
                    PrecoMedioMensal = g.Average(t => t.Precohora) * 176
                })
                .OrderBy(x => x.Categoria)
                .ThenBy(x => x.Pais)
                .ToListAsync();
        }
    }
}