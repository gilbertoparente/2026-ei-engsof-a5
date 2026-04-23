using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProfileMAnager.Controllers
{
    [Authorize] 
    public class SkillsController : Controller
    {
        
        private readonly IService<Skill> _skillRepo;
        private readonly IService<Areaprofissional> _areaRepo;

        public SkillsController(IService<Skill> skillRepo, IService<Areaprofissional> areaRepo)
        {
            _skillRepo = skillRepo;
            _areaRepo = areaRepo;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            
            var skills = await _skillRepo.Query()
                .Include(s => s.IdareaNavigation)
                .ToListAsync();
            return View(skills);
        }

        // CREATE (GET)
        public async Task<IActionResult> Create()
        {
            var areas = await _areaRepo.GetAllAsync();
            ViewBag.Idarea = new SelectList(areas, "Idarea", "Nome");
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Skill skill)
        {
            if (ModelState.IsValid)
            {
                await _skillRepo.AddAsync(skill);
                return RedirectToAction(nameof(Index));
            }
            var areas = await _areaRepo.GetAllAsync();
            ViewBag.Idarea = new SelectList(areas, "Idarea", "Nome", skill.Idarea);
            return View(skill);
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _skillRepo.GetByIdAsync(id.Value);
            if (skill == null) return NotFound();

            var areas = await _areaRepo.GetAllAsync();
            ViewBag.Idarea = new SelectList(areas, "Idarea", "Nome", skill.Idarea);
            return View(skill);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Skill skill)
        {
            if (id != skill.Idskill) return NotFound();

            if (ModelState.IsValid)
            {
                await _skillRepo.UpdateAsync(skill);
                return RedirectToAction(nameof(Index));
            }
            var areas = await _areaRepo.GetAllAsync();
            ViewBag.Idarea = new SelectList(areas, "Idarea", "Nome", skill.Idarea);
            return View(skill);
        }

        
        // GET: Skills/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var skill = await _skillRepo.Query()
                .Include(s => s.IdareaNavigation)
                .Include(s => s.Talentoskills) 
                .FirstOrDefaultAsync(m => m.Idskill == id);

            if (skill == null) return NotFound();
            
            ViewBag.PodeApagar = !skill.Talentoskills.Any();

            return View(skill);
        }

        //  POST: Skills/Delete

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skill = await _skillRepo.Query()
                .Include(s => s.Talentoskills)
                .FirstOrDefaultAsync(m => m.Idskill == id);

            if (skill == null) return NotFound();

            if (skill.Talentoskills.Any())
            {
                TempData["Erro"] = "Não pode apagar esta skill pois ela está associada a talentos.";
                return RedirectToAction(nameof(Index));
            }

            await _skillRepo.DeleteAsync(skill);
            TempData["Sucesso"] = "Skill eliminada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}