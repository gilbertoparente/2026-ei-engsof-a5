using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public class RelatorioContext
    {
        private IRelatorioStrategy _strategy;

        public RelatorioContext(IRelatorioStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IRelatorioStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task<List<RelatorioCategoriaPaisVM>> ExecutarRelatorio(
            string categoria,
            string pais,
            string skill)
        {
            return await _strategy.GerarRelatorio(
                categoria,
                pais,
                skill);
        }
    }
}