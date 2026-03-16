using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class TalentosController : Controller
    {
        private readonly AppDbContext _context;

        public TalentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Talentos/Index
       public IActionResult Index()  // SÍNCRONO TOTAL
       {
           var talentos = _context.Talentos.AsNoTracking().ToList();
           return View(talentos);
       }



        // GET: Talentos/Create
        public IActionResult Create()
        {
            ViewBag.Idcategorias = new SelectList(_context.Categoriatalentos, "Idcategoria", "Nome");
            ViewBag.Idutilizadores = new SelectList(_context.Set<Utilizador>(), "Idutilizador", "Nome");
            return View();
        }

        // POST: Talentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Talento talento)
        {
            if (ModelState.IsValid)
            {
                talento.CreatedAt = DateTime.Now;
                talento.UpdatedAt = DateTime.Now;
                _context.Add(talento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Idcategorias = new SelectList(_context.Categoriatalentos, "Idcategoria", "Nome", talento.Idcategoria);
            ViewBag.Idutilizadores = new SelectList(_context.Set<Utilizador>(), "Idutilizador", "Nome", talento.Idutilizador);
            return View(talento);
        }

        // GET: Talentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var talento = await _context.Talentos.FindAsync(id);
            if (talento == null) return NotFound();

            ViewBag.Idcategorias = new SelectList(_context.Categoriatalentos, "Idcategoria", "Nome", talento.Idcategoria);
            ViewBag.Idutilizadores = new SelectList(_context.Set<Utilizador>(), "Idutilizador", "Nome", talento.Idutilizador);
            return View(talento);
        }

        // POST: Talentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Talento talento)
        {
            if (id != talento.Idtalento) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    talento.UpdatedAt = DateTime.Now;
                    _context.Update(talento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TalentoExists(talento.Idtalento))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Idcategorias = new SelectList(_context.Categoriatalentos, "Idcategoria", "Nome", talento.Idcategoria);
            ViewBag.Idutilizadores = new SelectList(_context.Set<Utilizador>(), "Idutilizador", "Nome", talento.Idutilizador);
            return View(talento);
        }

        // GET: Talentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var talento = await _context.Talentos
                .FirstOrDefaultAsync(m => m.Idtalento == id);
            if (talento == null) return NotFound();

            return View(talento);
        }

        // POST: Talentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var talento = await _context.Talentos.FindAsync(id);
            if (talento != null)
            {
                _context.Talentos.Remove(talento);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // MÉTODO OBRIGATÓRIO - ADICIONADO!
        private bool TalentoExists(int id)
        {
            return _context.Talentos.Any(e => e.Idtalento == id);
        }
    }
}
