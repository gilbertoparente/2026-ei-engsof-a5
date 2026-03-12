using ProfileMAnager.Data;
using ProfileMAnager.Models;
using System.Linq;

namespace ProfileMAnager.Services
{
    public class AuthService
    {
        private readonly AppDBContext _context;

        public AuthService(AppDBContext context)
        {
            _context = context;
        }

        public bool Register(string nome, string email, string password)
        {
            if (_context.Utilizador.Any(u => u.Email == email))
                return false;

            var user = new Utilizador
            {
                Nome = nome,
                Email = email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Utilizador.Add(user);
            _context.SaveChanges();

            return true;
        }

        public Utilizador? Login(string email, string password)
        {
            var user = _context.Utilizador.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return null;

            bool valid = BCrypt.Net.BCrypt.Verify(password, user.Passwordhash);

            return valid ? user : null;
        }
    }
}