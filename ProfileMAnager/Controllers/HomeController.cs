using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProfileMAnager.Services;
using ProfileMAnager.Models.ViewModels;
using ProfileMAnager.Models;

namespace ProfileMAnager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [AllowAnonymous] 
        public async Task<IActionResult> Index()
        {
            // 1. Verifica se o utilizador está logado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                try 
                {
                    // Se estiver logado, carrega os dados reais do Dashboard
                    var model = await _dashboardService.GetDashboardDataAsync();
                    model.NomeUtilizador = User.Identity.Name ?? "Utilizador";
    
                    return View(model); // Carrega automaticamente o Index.cshtml (Dashboard)
                }
                catch (UnauthorizedAccessException ex)
                {
                    TempData["MensagemProxy"] = ex.Message; 
                    return RedirectToAction("Login", "Conta");
                }
            }

            // 2. CASO CONTRÁRIO (Se for Visitante/Não Logado)
            var modelVazio = new DashboardViewModel 
            { 
                NovosTalentos = new List<Talento>(), 
                NovasPropostas = new List<Propostatrabalho>(), 
                NovosClientes = new List<Cliente>(),
                NomeUtilizador = "Visitante"
            };

            // CORREÇÃO CRÍTICA AQUI: Forçar a renderização da View "Landing" em vez da "Index"
            return View("Landing", modelVazio);
        }
    }
}