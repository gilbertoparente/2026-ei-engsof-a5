using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Services;
using ProfileMAnager.Models;

namespace ProfileMAnager.Controllers
{
    public class UtizadorController : Controller
    {
        private readonly AuthService _authService;

        public UtizadorController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _authService.Login(email, password);

            if (user == null)
            {
                ViewBag.Error = "Email ou password inválidos";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string nome, string email, string password)
        {
            bool success = _authService.Register(nome, email, password);

            if (!success)
            {
                ViewBag.Error = "Email já registado";
                return View();
            }

            return RedirectToAction("Login");
        }
    }
}