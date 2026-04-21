using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace ProfileMAnager.Controllers
{
    public class CategoriatalentoController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriatalentoController(AppDbContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categoriatalentos.ToListAsync());
        }

        // INSERIR (GET)
        public IActionResult Create() => View();

        // INSERIR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoriatalento categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // ALTERAR (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var categoria = await _context.Categoriatalentos.FindAsync(id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        // ALTERAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoriatalento categoria)
        {
            if (id != categoria.Idcategoria) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        //delete categoria
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categoriatalentos
                .Include(c => c.Talentos) // Inclui os talentos para verificar se existem
                .Include(c => c.Propostatrabalhos) // Se as propostas também usarem esta categoria
                .FirstOrDefaultAsync(c => c.Idcategoria == id);

            if (categoria == null) return NotFound();

            // Verifica se existem dependências
            if (categoria.Talentos.Any() || categoria.Propostatrabalhos.Any())
            {
                // Aqui podes usar o TempData para enviar uma mensagem de erro para a Index
                TempData["Error"] = "Não pode eliminar esta categoria porque existem talentos ou propostas associados a ela.";
                return RedirectToAction(nameof(Index));
            }

            _context.Categoriatalentos.Remove(categoria);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Categoria eliminada com sucesso!";
    
            return RedirectToAction(nameof(Index));
        }
    }
}