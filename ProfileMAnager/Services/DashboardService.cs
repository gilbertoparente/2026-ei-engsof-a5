using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models.ViewModels;

namespace ProfileMAnager.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }
        
        
        public Task<DashboardViewModel> GetDashboardDataAsync()
        {
            throw new NotImplementedException("O serviço real requer um ID de utilizador. Utilize o Proxy para acesso automático ou a sobrecarga com parâmetro.");
        }
        
        /// Este é o método que o Proxy irá invocar após validar a segurança e extrair o ID.
       
        public async Task<DashboardViewModel> GetDashboardDataAsync(int userId)
        {
            return new DashboardViewModel
            {
                TotalTalentos = await _context.Talentos.CountAsync(),
                TotalPropostas = await _context.Propostatrabalhos.CountAsync(),
                TotalClientes = await _context.Clientes.CountAsync(),
                
                NovosTalentos = await _context.Talentos
                    .Where(t => t.Idutilizador == userId)
                    .OrderByDescending(t => t.Idtalento)
                    .Take(5).ToListAsync(),

                NovasPropostas = await _context.Propostatrabalhos
                    .OrderByDescending(p => p.Idproposta)
                    .Take(5).ToListAsync(),

                NovosClientes = await _context.Clientes
                    .OrderByDescending(c => c.Idcliente)
                    .Take(5).ToListAsync()
            };
        }
    }
}