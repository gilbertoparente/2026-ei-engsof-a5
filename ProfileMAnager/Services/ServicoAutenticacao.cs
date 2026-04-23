using ProfileMAnager.Data;
using ProfileMAnager.Models;
using BCrypt.Net;

namespace ProfileMAnager.Services
{
    // Adicionamos a herança da Interface que criámos no passo anterior
    public class ServicoAutenticacao : IAutenticacaoService
    {
        private readonly AppDbContext _context;

        public ServicoAutenticacao(AppDbContext context)
        {
            _context = context;
        }

        public bool Registar(string nome, string email, string password)
        {
            // Verifica se o email já existe (boa prática)
            if (_context.Utilizadors.Any(u => u.Email == email))
                return false;

            var utilizador = new Utilizador
            {
                Nome = nome,
                Email = email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow, 
                UpdatedAt = DateTime.UtcNow  
            };

            _context.Utilizadors.Add(utilizador);
            _context.SaveChanges();
            return true;
        }

        // Alterado de 'Login' para 'Autenticar' para bater certo com a Interface
        public Utilizador? Autenticar(string email, string password)
        {
            var utilizador = _context.Utilizadors.FirstOrDefault(u => u.Email == email);
            if (utilizador == null)
                return null;

            // Verifica se a password em texto limpo corresponde ao Hash na BD
            bool valido = BCrypt.Net.BCrypt.Verify(password, utilizador.Passwordhash);
            return valido ? utilizador : null;
        }
    }
}