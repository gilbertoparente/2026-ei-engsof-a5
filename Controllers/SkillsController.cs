using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProfileMAnager.Controllers
{
    [Authorize] 
    public class SkillsController : Controller
    {
        private readonly AppDbContext _context;

        public SkillsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var skills = await _context.Skills
                .Include(s => s.IdareaNavigation)
                .ToListAsync();
            return View(skills);
        }

        //criar
        public IActionResult Create()
        {
            ViewBag.Idarea = new SelectList(_context.Areaprofissionals, "Idarea", "Nome");
            return View();
        }

        // criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Skill skill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Idarea = new SelectList(_context.Areaprofissionals, "Idarea", "Nome", skill.Idarea);
            return View(skill);
        }

        //  Editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();

            ViewBag.Idarea = new SelectList(_context.Areaprofissionals, "Idarea", "Nome", skill.Idarea);
            return View(skill);
        }

        // Editar 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Skill skill)
        {
            if (id != skill.Idskill) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Idarea = new SelectList(_context.Areaprofissionals, "Idarea", "Nome", skill.Idarea);
            return View(skill);
        }

        // delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _context.Skills
                .Include(s => s.IdareaNavigation)
                .FirstOrDefaultAsync(m => m.Idskill == id);

            if (skill == null) return NotFound();

            bool temTalentos = await _context.Talentoskills.AnyAsync(ts => ts.Idskill == id);
            ViewBag.PodeApagar = !temTalentos;

            return View(skill);
        }

        // delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            
            bool temTalentos = await _context.Talentoskills.AnyAsync(ts => ts.Idskill == id);
            
            if (temTalentos)
            {
                TempData["Erro"] = "Não pode apagar esta skill pois ela está associada a talentos.";
                return RedirectToAction(nameof(Index));
            }

            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}