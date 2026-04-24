using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProfileMAnager.Services;
using ProfileMAnager.Models.ViewModels;
using System.Security.Claims;
using ProfileMAnager.Models; // Certifica-te que tens este using para as Listas

namespace ProfileMAnager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Método único para a Home
        [AllowAnonymous] 
        public async Task<IActionResult> Index()
        {
            // 1. Se o utilizador estiver logado, vamos buscar os dados reais
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userId = int.TryParse(userIdStr, out int id) ? id : 0;
        
                var model = await _dashboardService.GetDashboardDataAsync(userId);
                model.NomeUtilizador = User.Identity.Name ?? "Utilizador";
        
                return View(model); 
            }

            // 2. Se NÃO estiver logado, enviamos um Model vazio para evitar erros na View
            // A View (Index.cshtml) vai detetar que não há login e mostrar a Landing Page
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