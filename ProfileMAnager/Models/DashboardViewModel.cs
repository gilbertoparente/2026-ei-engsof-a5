using ProfileMAnager.Models; 

namespace ProfileMAnager.Models.ViewModels
{
    public class DashboardViewModel
    {
        public string NomeUtilizador { get; set; } = string.Empty;
        public int TotalTalentos { get; set; }
        public int TotalPropostas { get; set; }
        public int TotalClientes { get; set; }
        
        public List<Talento> NovosTalentos { get; set; } = new();
        public List<Propostatrabalho> NovasPropostas { get; set; } = new();
        public List<Cliente> NovosClientes { get; set; } = new();
    }
}