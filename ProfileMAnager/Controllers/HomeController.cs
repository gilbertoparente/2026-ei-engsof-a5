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
            //  verifica se está logado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                try 
                {
                    // chama o serviço.
                    var model = await _dashboardService.GetDashboardDataAsync();
                    model.NomeUtilizador = User.Identity.Name ?? "Utilizador";
            
                    return View(model); 
                }
                catch (UnauthorizedAccessException ex)
                {
                   
                    TempData["MensagemProxy"] = ex.Message; 
                    return RedirectToAction("Login", "Conta");
                }
            }

            // Caso contrário, mostra a Landing Page
            var modelVazio = new DashboardViewModel 
            { 
                NovosTalentos = new List<Talento>(), 
                NovasPropostas = new List<Propostatrabalho>(), 
                NovosClientes = new List<Cliente>(),
                NomeUtilizador = "Visitante"
            };

            return View(modelVazio);
        }
    }
}