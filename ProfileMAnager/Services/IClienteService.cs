using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllByUserIdAsync(int userId);
        Task<Cliente?> GetByIdAsync(int id);
        Task AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);
        Task<bool> ExisteAsync(int id);
    }
}