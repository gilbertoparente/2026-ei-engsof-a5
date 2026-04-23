using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Services;
using ProfileMAnager.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class PesquisaController : Controller
    {
        private readonly IPesquisaService _pesquisaService;

        public PesquisaController(IPesquisaService pesquisaService)
        {
            _pesquisaService = pesquisaService;
        }
        // get lista
        public async Task<IActionResult> Index(int[] skillIds)
        {
            
            var todasAsSkills = await _pesquisaService.GetListaSkillsOrdenadaAsync();
            ViewBag.Skills = todasAsSkills;
            
            ViewBag.SelectedSkills = skillIds ?? Array.Empty<int>();
            if (skillIds == null || skillIds.Length == 0)
            {
                return View(new List<Talento>());
            }

            
            var resultados = await _pesquisaService.PesquisarTalentosPorSkillsAsync(skillIds);
            
            return View(resultados);
        }
    }
}