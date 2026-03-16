using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProfileMAnager.Controllers
{
    [Authorize] // Apenas utilizadores logados podem gerir skills
    public class SkillsController : Controller
    {
        private readonly AppDbContext _context;

        public SkillsController(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as skills
        public async Task<IActionResult> Index()
        {
            var skills = await _context.Skills
                .Include(s => s.IdareaNavigation)
                .ToListAsync();
            return View(skills);
        }

        // GET: Criar Skill
        public IActionResult Create()
        {
            // Carrega as áreas profissionais para a dropdown
            ViewBag.Idarea = new SelectList(_context.Areaprofissionals, "Idarea", "Nome");
            return View();
        }

        // POST: Criar Skill
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

        // GET: Editar Skill
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();

            ViewBag.Idarea = new SelectList(_context.Areaprofissionals, "Idarea", "Nome", skill.Idarea);
            return View(skill);
        }

        // POST: Editar Skill
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

        // GET: Apagar Skill (Página de confirmação)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _context.Skills
                .Include(s => s.IdareaNavigation)
                .FirstOrDefaultAsync(m => m.Idskill == id);

            if (skill == null) return NotFound();

            // REGRA: Verificar se a skill está associada a algum talento
            bool temTalentos = await _context.Talentoskills.AnyAsync(ts => ts.Idskill == id);
            ViewBag.PodeApagar = !temTalentos;

            return View(skill);
        }

        // POST: Confirmar Apagar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            
            // Verificação extra de segurança no servidor
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