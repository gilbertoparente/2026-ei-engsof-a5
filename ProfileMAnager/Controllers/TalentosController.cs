using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using System.Security.Claims;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class TalentosController : Controller
    {
        private readonly ITalentoService _talentoService;
        private readonly IService<Categoriatalento> _categoriaRepo;
        private readonly IService<Skill> _skillRepo;

        public TalentosController(
            ITalentoService talentoService, 
            IService<Categoriatalento> categoriaRepo,
            IService<Skill> skillRepo)
        {
            _talentoService = talentoService;
            _categoriaRepo = categoriaRepo;
            _skillRepo = skillRepo;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            int userId = GetCurrentUserId();
            var talentos = await _talentoService.GetTalentosPorUtilizadorAsync(userId);
            return View(talentos);
        }

        // DETALHES
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var talento = await _talentoService.GetDetalhesCompletosAsync(id.Value);
            if (talento == null) return NotFound();

            return View(talento);
        }

        // CRIAR (GET)
        public async Task<IActionResult> Create()
        {
            ViewBag.Idcategoria = new SelectList(await _categoriaRepo.GetAllAsync(), "Idcategoria", "Nome");
            return View();
        }

        // CRIAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Talento talento)
        {
            int userId = GetCurrentUserId();
            
            ModelState.Remove("IdutilizadorNavigation");
            ModelState.Remove("IdcategoriaNavigation");

            if (ModelState.IsValid)
            {
                int novoId = await _talentoService.CriarTalentoAsync(talento, userId);
                return RedirectToAction(nameof(Details), new { id = novoId });
            }

            ViewBag.Idcategoria = new SelectList(await _categoriaRepo.GetAllAsync(), "Idcategoria", "Nome", talento.Idcategoria);
            return View(talento);
        }

        // ADICIONAR SKILL (GET)
        public async Task<IActionResult> AdicionarSkill(int id)
        {
            var talento = await _talentoService.GetDetalhesCompletosAsync(id);
            if (talento == null) return NotFound();

            ViewBag.Idskill = new SelectList(await _skillRepo.GetAllAsync(), "Idskill", "Nome");
            return View(new Talentoskill { Idtalento = id });
        }

        // ADICIONAR SKILL (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarSkill(Talentoskill ts)
        {
            ModelState.Remove("IdskillNavigation");
            ModelState.Remove("IdtalentoNavigation");

            if (ModelState.IsValid)
            {
                var erro = await _talentoService.AdicionarSkillAsync(ts);
                if (erro == null)
                    return RedirectToAction(nameof(Details), new { id = ts.Idtalento });

                ViewBag.Erro = erro;
            }

            ViewBag.Idskill = new SelectList(await _skillRepo.GetAllAsync(), "Idskill", "Nome", ts.Idskill);
            return View(ts);
        }

        // ADICIONAR EXPERIÊNCIA (GET)
        public IActionResult AdicionarExperiencia(int id)
        {
            return View(new Experiencia { Idtalento = id });
        }

        // ADICIONAR EXPERIÊNCIA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarExperiencia(Experiencia exp)
        {
            ModelState.Remove("IdtalentoNavigation");

            if (ModelState.IsValid)
            {
                var erro = await _talentoService.AdicionarExperienciaAsync(exp);
                if (erro == null)
                    return RedirectToAction(nameof(Details), new { id = exp.Idtalento });

                ViewBag.Erro = erro;
            }
            return View(exp);
        }

        
        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out int id) ? id : 0;
        }
        
        // EDITAR (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var talento = await _talentoService.GetDetalhesCompletosAsync(id.Value);
            if (talento == null || talento.Idutilizador != GetCurrentUserId()) return NotFound();

            ViewBag.Idcategoria = new SelectList(await _categoriaRepo.GetAllAsync(), "Idcategoria", "Nome", talento.Idcategoria);
            return View(talento);
        }

        // EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Talento talento)
        {
            
            if (id != talento.Idtalento) return NotFound();
            ModelState.Remove("Email");
            ModelState.Remove("IdutilizadorNavigation");
            ModelState.Remove("IdcategoriaNavigation");
            ModelState.Remove("Talentoskills");
            ModelState.Remove("Experiencias");

            if (ModelState.IsValid)
            {
                try
                {
                    await _talentoService.AtualizarTalentoAsync(talento);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao guardar: " + ex.Message);
                }
            }
            ViewBag.Idcategoria = new SelectList(await _categoriaRepo.GetAllAsync(), "Idcategoria", "Nome", talento.Idcategoria);
    
            
            return View(talento);
        }

        // ELIMINAR GET 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var talento = await _talentoService.GetDetalhesCompletosAsync(id.Value);
            if (talento == null || talento.Idutilizador != GetCurrentUserId()) return NotFound();

            return View(model: talento);
        }

        // ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _talentoService.EliminarTalentoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}