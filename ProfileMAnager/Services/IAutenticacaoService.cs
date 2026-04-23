using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public interface IAutenticacaoService
    {
        Utilizador? Autenticar(string email, string password);
        bool Registar(string nome, string email, string password);
    }
}