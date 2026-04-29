using System.Security.Claims;
using ProfileMAnager.Models.ViewModels;

namespace ProfileMAnager.Services
{
    public class DashboardServiceProxy : IDashboardService
    {
        private readonly DashboardService _realService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardServiceProxy(DashboardService realService, IHttpContextAccessor httpContextAccessor)
        {
            _realService = realService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            //  verificar a segurança
            var user = _httpContextAccessor.HttpContext?.User;
            
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("Proxy: Acesso negado. Utilizador não autenticado.");
            }
            var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
            {
                throw new UnauthorizedAccessException("Proxy: ID de utilizador inválido.");
            }
            
            return await _realService.GetDashboardDataAsync(userId);
        }
    }
}