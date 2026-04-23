using ProfileMAnager.Models;
using ProfileMAnager.Models.ViewModels;

namespace ProfileMAnager.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync(int userId);
    }
}