using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProfileMAnager.Services;
using ProfileMAnager.Models.ViewModels;
using System.Security.Claims;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.TryParse(userIdStr, out int id) ? id : 0;
            
            var model = await _dashboardService.GetDashboardDataAsync(userId);
            
            model.NomeUtilizador = User.Identity?.Name ?? "Utilizador";
            
            return View(model);
        }
    }
}