using ProfileMAnager.Data;
using ProfileMAnager.Models;
using System.Linq;

namespace ProfileMAnager.Services
{
    public class ServicoAutenticacao
    {
        private readonly AppDbContext _context;

        public ServicoAutenticacao(AppDbContext context)
        {
            _context = context;
        }

        public bool Registar(string nome, string email, string password)
        {
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

        public Utilizador? Login(string email, string password)
        {
            var utilizador = _context.Utilizadors.FirstOrDefault(u => u.Email == email);
            if (utilizador == null)
                return null;

            bool valido = BCrypt.Net.BCrypt.Verify(password, utilizador.Passwordhash);
            return valido ? utilizador : null;
        }
    }
}