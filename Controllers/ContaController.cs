using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ProfileMAnager.Controllers
{
    public class ContaController : Controller
    {
        private readonly ServicoAutenticacao _servico;

        public ContaController(ServicoAutenticacao servico)
        {
            _servico = servico;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var utilizador = _servico.Login(email, password);
            if (utilizador == null)
            {
                ViewBag.Erro = "Email ou password inválidos";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilizador.Idutilizador.ToString()),
                new Claim(ClaimTypes.Name, utilizador.Nome),
                new Claim(ClaimTypes.Email, utilizador.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registo() => View();

        [HttpPost]
        public IActionResult Registo(string nome, string email, string password)
        {
            bool sucesso = _servico.Registar(nome, email, password);
            if (!sucesso)
            {
                ViewBag.Erro = "Email já registado";
                return View();
            }

            // MENSAGEM DE SUCESSO AQUI:
            TempData["Sucesso"] = "Conta criada com sucesso! Já pode fazer login.";

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}