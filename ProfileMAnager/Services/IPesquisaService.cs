using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public interface IPesquisaService
    {
        Task<IEnumerable<Talento>> PesquisarTalentosPorSkillsAsync(int[] skillIds);
        Task<IEnumerable<Skill>> GetListaSkillsOrdenadaAsync();
    }
}