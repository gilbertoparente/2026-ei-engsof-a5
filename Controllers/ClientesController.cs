using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // listar 
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var clientes = await _context.Clientes
                .Where(c => c.Idutilizador == userId)
                .ToListAsync();
            return View(clientes);
        }

        // criar view
        public IActionResult Create()
        {
            return View();
        }

        // criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            cliente.Idutilizador = userId;
            cliente.CreatedAt = DateTime.UtcNow;
            cliente.UpdatedAt = DateTime.UtcNow;

            ModelState.Remove("IdutilizadorNavigation");

            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (cliente.Idutilizador != userId) return Forbid();

            return View(cliente);
        }

        // editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Idcliente) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            cliente.Idutilizador = userId;
            cliente.UpdatedAt = DateTime.UtcNow;
            ModelState.Remove("IdutilizadorNavigation");
            if (ModelState.IsValid)
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Idcliente == id);
            if (cliente == null) return NotFound();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (cliente.Idutilizador != userId) return Forbid();

            return View(cliente);
        }

        // delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}