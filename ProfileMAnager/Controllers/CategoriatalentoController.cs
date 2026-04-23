using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class CategoriatalentoController : Controller
    {
        private readonly IService<Categoriatalento> _repo;

        public CategoriatalentoController(IService<Categoriatalento> repo)
        {
            _repo = repo;
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            var lista = await _repo.GetAllAsync();
            return View(lista);
        }
        public IActionResult Create() => View();

        // INSERIR  grava
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoriatalento categoria)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // ALTERAR (GET): Procura a categoria pelo ID
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _repo.GetByIdAsync(id.Value);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // ALTERAR (POST): Atualiza os dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoriatalento categoria)
        {
            if (id != categoria.Idcategoria) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateAsync(categoria);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Erro de concorrência ao atualizar a categoria.");
                }
            }
            return View(categoria);
        }

        // GET: Categoriatalento 

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var categoria = await _repo.Query()
                .Include(c => c.Talentos)
                .Include(c => c.Propostatrabalhos)
                .FirstOrDefaultAsync(m => m.Idcategoria == id);

            if (categoria == null) return NotFound();
            
            ViewBag.PodeApagar = !categoria.Talentos.Any() && !categoria.Propostatrabalhos.Any();

            return View(categoria);
        }

        // POST: Categoriatalento/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _repo.Query()
                .Include(c => c.Talentos)
                .Include(c => c.Propostatrabalhos)
                .FirstOrDefaultAsync(c => c.Idcategoria == id);

            if (categoria == null) return NotFound();

            if (categoria.Talentos.Any() || categoria.Propostatrabalhos.Any())
            {
                TempData["Error"] = "Impossível eliminar: Existem registos associados.";
                return RedirectToAction(nameof(Index));
            }

            await _repo.DeleteAsync(categoria);
            TempData["Success"] = "Categoria eliminada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}