using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ProfileMAnager.Controllers
{
    public class ContaController : Controller
    {
        private readonly IAutenticacaoService _authService;

        public ContaController(IAutenticacaoService authService)
        {
            _authService = authService;
        }

        public IActionResult Login() => View();

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

        public IActionResult Registo() => View();

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
    }
}