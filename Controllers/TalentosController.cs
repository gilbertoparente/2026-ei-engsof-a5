using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        // Listar
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var talentos = await _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Where(t => t.Idutilizador == userId)
                .ToListAsync();
                
            return View(talentos);
        }
        //  Criar
        public IActionResult Create()
        {
            ViewBag.Idcategoria = new SelectList(_context.Categoriatalentos, "Idcategoria", "Nome");
            return View();
        }

        //  criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Talento talento)
        {
            
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            talento.Idutilizador = userId;
            talento.CreatedAt = DateTime.UtcNow;
            talento.UpdatedAt = DateTime.UtcNow;
           
            ModelState.Remove("IdutilizadorNavigation");
            ModelState.Remove("IdcategoriaNavigation");

            if (ModelState.IsValid)
            {
                _context.Add(talento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = talento.Idtalento });
            }

            ViewBag.Idcategoria = new SelectList(_context.Categoriatalentos, "Idcategoria", "Nome", talento.Idcategoria);
            return View(talento);
        }

        
        public async Task<IActionResult> Details(int? id) 
        {
            if (id == null) return NotFound();

            var talento = await _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Include(t => t.Talentoskills).ThenInclude(ts => ts.IdskillNavigation)
                .FirstOrDefaultAsync(m => m.Idtalento == id);

            if (talento == null) return NotFound();

            return View(talento);
        }
        
        public async Task<IActionResult> AdicionarSkill(int id)
        {
            var talento = await _context.Talentos.FindAsync(id);
            if (talento == null) return NotFound();

         
            ViewBag.Idskill = new SelectList(_context.Skills, "Idskill", "Nome");
    
           
            var model = new Talentoskill { Idtalento = id };
    
            return View(model);
        }

        //Adicionar  skill
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarSkill(Talentoskill ts)
        {
            
            ModelState.Remove("IdskillNavigation");
            ModelState.Remove("IdtalentoNavigation");

            if (ModelState.IsValid)
            {
                
                var existe = await _context.Talentoskills
                    .AnyAsync(x => x.Idtalento == ts.Idtalento && x.Idskill == ts.Idskill);

                if (existe)
                {
                    ViewBag.Erro = "Este talento já possui esta skill associada.";
                    ViewBag.Idskill = new SelectList(_context.Skills, "Idskill", "Nome", ts.Idskill);
                    return View(ts);
                }

                _context.Talentoskills.Add(ts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = ts.Idtalento });
            }

            ViewBag.Idskill = new SelectList(_context.Skills, "Idskill", "Nome", ts.Idskill);
            return View(ts);
        }
        
        //  Adicionar experiencia 
        public async Task<IActionResult> AdicionarExperiencia(int id)
        {
            var talento = await _context.Talentos.FindAsync(id);
            if (talento == null) return NotFound();

            var model = new Experiencia { Idtalento = id };
            return View(model);
        }

        // Adicionar  experiencia 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarExperiencia(Experiencia exp)
        {
            ModelState.Remove("IdtalentoNavigation");

            if (ModelState.IsValid)
            {
                var sobreposicao = await _context.Experiencia
                    .Where(e => e.Idtalento == exp.Idtalento)
                    .AnyAsync(e => 
                        (exp.Anoinicio >= e.Anoinicio && exp.Anoinicio <= (e.Anofim ?? DateTime.Now.Year)) ||
                        (exp.Anofim != null && exp.Anofim >= e.Anoinicio && exp.Anofim <= (e.Anofim ?? DateTime.Now.Year))
                    );

                if (sobreposicao)
                {
                    ViewBag.Erro = "Não pode haver sobreposição de experiências no mesmo ano. Verifique as datas.";
                    return View(exp);
                }

                exp.CreatedAt = DateTime.UtcNow;
                exp.UpdatedAt = DateTime.UtcNow;
                _context.Experiencia.Add(exp);
                await _context.SaveChangesAsync();
        
                return RedirectToAction(nameof(Details), new { id = exp.Idtalento });
            }

            return View(exp);
        }
    }
}