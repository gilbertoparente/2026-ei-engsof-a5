using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public interface ITalentoService
    {
        Task<IEnumerable<Talento>> GetTalentosPorUtilizadorAsync(int userId);
        Task<Talento?> GetDetalhesCompletosAsync(int id);
        Task<int> CriarTalentoAsync(Talento talento, int userId);
        Task<string?> AdicionarSkillAsync(Talentoskill ts);
        Task<string?> AdicionarExperienciaAsync(Experiencia exp);
        
        Task AtualizarTalentoAsync(Talento talento);
        
        Task EliminarTalentoAsync(int id);
    }
}