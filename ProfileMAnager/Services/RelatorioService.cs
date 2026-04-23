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

        public async Task<List<RelatorioCategoriaPaisVM>> GetRelatorioCategoriaPais()
        {
            return await _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .GroupBy(t => new
                {
                    Categoria = t.IdcategoriaNavigation.Nome,
                    Pais = t.Pais
                })
                .Select(g => new RelatorioCategoriaPaisVM
                {
                    Categoria = g.Key.Categoria,
                    Pais = g.Key.Pais,
                    Total = g.Count()
                })
                .OrderBy(r => r.Categoria)
                .ThenBy(r => r.Pais)
                .ToListAsync();
        }
    }
}