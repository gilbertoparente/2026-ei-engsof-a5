using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Services;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        private readonly AppDbContext _context;

        private readonly RelatorioCategoriaPaisStrategy _categoriaStrategy;
        private readonly RelatorioSkillStrategy _skillStrategy;

        public RelatoriosController(
            AppDbContext context,
            RelatorioCategoriaPaisStrategy categoriaStrategy,
            RelatorioSkillStrategy skillStrategy)
        {
            _context = context;

            _categoriaStrategy = categoriaStrategy;
            _skillStrategy = skillStrategy;
        }

        public async Task<IActionResult> RelatorioCategoriaPais(
            string? categoria,
            string? pais,
            string? skill)
        {
            // Categorias
            ViewBag.Categorias = await _context.Categoriatalentos
                .Select(c => c.Nome)
                .Distinct()
                .ToListAsync();

            // Países
            ViewBag.Paises = await _context.Talentos
                .Select(t => t.Pais)
                .Distinct()
                .ToListAsync();

            // Skills
            ViewBag.Skills = await _context.Skills
                .Select(s => s.Nome)
                .Distinct()
                .ToListAsync();

            // Strategy Pattern
            var context = new RelatorioContext(_skillStrategy);

            var resultado =
                await context.ExecutarRelatorio(
                    categoria,
                    pais,
                    skill);

            return View(resultado);
        }
    }
}