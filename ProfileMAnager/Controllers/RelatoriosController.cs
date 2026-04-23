using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Services;

namespace ProfileMAnager.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly RelatorioService _relatorioService;
        private readonly AppDbContext _context;

        public RelatoriosController(RelatorioService relatorioService, AppDbContext context)
        {
            _relatorioService = relatorioService;
            _context = context;
        }

        public async Task<IActionResult> RelatorioCategoriaPais(string categoria, string pais)
        {
            ViewBag.Categorias = await _context.Categoriatalentos
                .Select(c => c.Nome)
                .Distinct()
                .ToListAsync();

            ViewBag.Paises = await _context.Talentos
                .Select(t => t.Pais)
                .Distinct()
                .ToListAsync();

            var resultado = await _relatorioService.GetRelatorioCategoriaPais(categoria, pais);

            return View(resultado);
        }
    }
}