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

        
        //pesquisa talentos
        public async Task<IActionResult> Index(int[] skillIds)
        {
            
            ViewBag.Skills = await _context.Skills.OrderBy(s => s.Nome).ToListAsync();
            var query = _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Where(t => t.Publico == true);

            if (skillIds != null && skillIds.Length > 0)
            {
                foreach (var id in skillIds)
                {
                    query = query.Where(t => t.Talentoskills.Any(ts => ts.Idskill == id));
                }
            }
            else
            {
                ViewBag.SelectedSkills = new int[0];
                return View(new List<Talento>());
            }

            var resultados = await query.OrderBy(t => t.Nome).ToListAsync();
            ViewBag.SelectedSkills = skillIds;
            return View(resultados);
        }
    }
}