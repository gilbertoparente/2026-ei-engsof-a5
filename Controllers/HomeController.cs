using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using System.Security.Claims;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) 
        { 
            _context = context; 
        }
        //mostra dados
        public async Task<IActionResult> Index()
        {
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
            ViewBag.NomeUtilizador = User.Identity?.Name ?? "Utilizador";
            
            ViewBag.TotalTalentos = await _context.Talentos.CountAsync();
            ViewBag.TotalPropostas = await _context.Propostatrabalhos.CountAsync();
            ViewBag.TotalClientes = await _context.Clientes.CountAsync();
            ViewBag.NovosTalentos = await _context.Talentos
                .Where(t => t.Idutilizador == userId)
                .OrderByDescending(t => t.Idtalento)
                .Take(5)
                .ToListAsync();

           ViewBag.NovasPropostas = await _context.Propostatrabalhos
                .OrderByDescending(p => p.Idproposta)
                .Take(5)
                .ToListAsync();

           
            ViewBag.NovosClientes = await _context.Clientes
                .OrderByDescending(c => c.Idcliente)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}