using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public interface IPropostaService
    {
        Task<IEnumerable<Propostatrabalho>> GetPropostasPorUtilizadorAsync(int userId);
        Task<Propostatrabalho?> GetPropostaDetalhadaAsync(int id);
        Task CriarPropostaAsync(Propostatrabalho proposta, int[] skills, int[] anos);
        Task AtualizarPropostaAsync(int id, Propostatrabalho proposta, int[] skills, int[] anos);
        Task<IEnumerable<Talento>> GetMatchingTalentosAsync(int propostaId);
        Task<bool> AtribuirTalentoAsync(int idProposta, int idTalento);
        Task EliminarPropostaAsync(int id);
        
        Task AlterarEstadoTalentoAsync(int id, string estado);
        Task RemoverTalentoDaPropostaAsync(int id);
        Task<Propostatrabalho?> GetDetalhesAsync(int id);
    }
}