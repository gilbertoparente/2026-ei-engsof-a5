using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using System.Security.Claims;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IClienteService _service;
        
        public ClientesController(IClienteService service)
        {
            _service = service;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.TryParse(userIdStr, out int id) ? id : 0;
            
            var clientes = (await _service.GetAllByUserIdAsync(userId)).ToList();
    
            return View(clientes);
        }

        // GET: CREATE
        public IActionResult Create()
        {
            return View();
        }

        // POST: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cliente.Idutilizador = int.TryParse(userIdStr, out int id) ? id : 0;
            
            cliente.CreatedAt = DateTime.UtcNow;
            cliente.UpdatedAt = DateTime.UtcNow;

            ModelState.Remove("IdutilizadorNavigation");

            if (ModelState.IsValid)
            {
                await _service.AddAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            
            var cliente = await _service.GetByIdAsync(id.Value);
            
            if (cliente == null) return NotFound();
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.TryParse(userIdStr, out int idUser) ? idUser : 0;

            if (cliente.Idutilizador != userId) return Forbid();

            return View(cliente);
        }

        // POST: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Idcliente) return NotFound();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cliente.Idutilizador = int.TryParse(userIdStr, out int idUser) ? idUser : 0;
            cliente.UpdatedAt = DateTime.UtcNow;

            ModelState.Remove("IdutilizadorNavigation");

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var cliente = await _service.GetByIdAsync(id.Value);

            if (cliente == null) return NotFound();
            
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (cliente.Idutilizador != userId) return Forbid();

            return View(cliente);
        }

        // POST: Clientes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _service.GetByIdAsync(id);
    
            if (cliente == null) return NotFound();
            
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (cliente.Idutilizador != userId) return Forbid();

            await _service.DeleteAsync(id);
    
            TempData["Sucesso"] = "Cliente eliminado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}