using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProfileMAnager.Data;
using ProfileMAnager.Models.ViewModels;

namespace ProfileMAnager.Controllers
{
    public class ContaController : Controller
    {
        private readonly IAutenticacaoService _authService;
        private readonly AppDbContext _context;

        public ContaController(IAutenticacaoService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        
        // GET - Apresenta a página de login 
        
        public IActionResult Login() => View();

        //autenticaão com cookie
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Login(string email, string password)
        {
            var utilizador = _authService.Autenticar(email, password);
            
            if (utilizador == null)
            {
                ModelState.AddModelError("", "Email ou password inválidos.");
                return View();
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilizador.Idutilizador.ToString()),
                new Claim(ClaimTypes.Name, utilizador.Nome ?? "Utilizador"),
                new Claim(ClaimTypes.Email, utilizador.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        // GET - apresenta o registo
        public IActionResult Registo() => View();
        
        // POST realiza o regisot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registo(string nome, string email, string password)
        {
            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Erro = "Todos os campos são obrigatórios.";
                return View();
            }

            bool sucesso = _authService.Registar(nome, email, password);
            if (!sucesso)
            {
                ViewBag.Erro = "Este email já se encontra registado.";
                return View();
            }
          
            TempData["Sucesso"] = "Registo efetuado! Já pode entrar na sua conta.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return NotFound();

            var utilizador = await _context.Utilizadors.FindAsync(userId);
            if (utilizador == null) return NotFound();

            var model = new PerfilViewModel
            {
                Nome = utilizador.Nome,
                Email = utilizador.Email
            };

            return View(model);
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Perfil(PerfilViewModel model)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return NotFound();

            if (ModelState.IsValid)
            {
                var utilizador = await _context.Utilizadors.FindAsync(userId);
                if (utilizador == null) return NotFound();

                utilizador.Nome = model.Nome;
                utilizador.Email = model.Email;

                // Só atualiza a password se o utilizador escreveu algo no campo
                if (!string.IsNullOrEmpty(model.NovaPassword))
                {
                    // Nota: Numa app real, usarias BCrypt ou PasswordHasher aqui
                    utilizador.Passwordhash = model.NovaPassword; 
                }

                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                    TempData["Sucesso"] = "Perfil atualizado com sucesso!";
                    return RedirectToAction(nameof(Perfil));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao atualizar o perfil. O email pode já estar em uso.");
                }
            }
            return View(model);
        }
    }
}