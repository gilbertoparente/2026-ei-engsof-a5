using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class PesquisaController : Controller
    {
        private readonly AppDbContext _context;

        public PesquisaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Exibir a página de pesquisa com a lista de skills
        public async Task<IActionResult> Index(int[] skillIds)
        {
            // Carregar todas as skills para as checkboxes
            ViewBag.Skills = await _context.Skills.OrderBy(s => s.Nome).ToListAsync();
            
            // Base da query: apenas talentos públicos (Requisito 3)
            var query = _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Where(t => t.Publico == true);

            // Se o utilizador selecionou skills, filtramos (Requisito 7)
            if (skillIds != null && skillIds.Length > 0)
            {
                foreach (var id in skillIds)
                {
                    // Garante que o talento tem CADA uma das skills selecionadas (Lógica AND)
                    query = query.Where(t => t.Talentoskills.Any(ts => ts.Idskill == id));
                }
            }
            else
            {
                // Se ainda não selecionou nada, retornamos lista vazia inicialmente
                ViewBag.SelectedSkills = new int[0];
                return View(new List<Talento>());
            }

            // Executar a query com ordenação por Nome (Requisito 7)
            var resultados = await query.OrderBy(t => t.Nome).ToListAsync();

            ViewBag.SelectedSkills = skillIds;
            return View(resultados);
        }
    }
}