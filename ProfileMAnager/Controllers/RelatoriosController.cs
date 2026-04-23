using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Services;

namespace ProfileMAnager.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly RelatorioService _relatorioService;

        public RelatoriosController(RelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        public async Task<IActionResult> RelatorioCategoriaPais()
        {
            var relatorio = await _relatorioService.GetRelatorioCategoriaPais();

            return View(relatorio);
        }
    }
}