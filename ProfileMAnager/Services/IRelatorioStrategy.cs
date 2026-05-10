using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public interface IRelatorioStrategy
    {
        Task<List<RelatorioCategoriaPaisVM>> GerarRelatorio(
            string categoria,
            string pais,
            string skill);
    }
}