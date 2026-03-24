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

        // Listar apenas os talentos criados pelo utilizador logado
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var talentos = await _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Where(t => t.Idutilizador == userId)
                .ToListAsync();
                
            return View(talentos);
        }

        // GET: Criar Perfil de Talento
        public IActionResult Create()
        {
            ViewBag.Idcategoria = new SelectList(_context.Categoriatalentos, "Idcategoria", "Nome");
            return View();
        }

        // POST: Criar Perfil de Talento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Talento talento)
        {
            // Atribuir automaticamente o ID do utilizador logado
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            talento.Idutilizador = userId;
            talento.CreatedAt = DateTime.UtcNow;
            talento.UpdatedAt = DateTime.UtcNow;

            // Remover validação do Idutilizador e Navegações para que o ModelState seja válido
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

        // Ver Detalhes do Perfil (e onde vamos gerir Skills e Experiência mais tarde)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var talento = await _context.Talentos
                .Include(t => t.IdcategoriaNavigation)
                .Include(t => t.Talentoskills).ThenInclude(ts => ts.IdskillNavigation)
                .Include(t => t.Experiencia)
                .FirstOrDefaultAsync(m => m.Idtalento == id);

            if (talento == null) return NotFound();

            // Segurança: Verificar se o talento pertence ao utilizador logado
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (talento.Idutilizador != userId) return Forbid();

            return View(talento);
        }
        
        // GET: Adicionar Skill ao Talento
        public async Task<IActionResult> AdicionarSkill(int id)
        {
            var talento = await _context.Talentos.FindAsync(id);
            if (talento == null) return NotFound();

            // Carregar a lista de todas as skills disponíveis
            ViewBag.Idskill = new SelectList(_context.Skills, "Idskill", "Nome");
    
            // Criamos um objeto Talentoskill para passar para a View
            var model = new Talentoskill { Idtalento = id };
    
            return View(model);
        }

// POST: Adicionar Skill ao Talento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarSkill(Talentoskill ts)
        {
            // Remover navegações do ModelState para não dar erro
            ModelState.Remove("IdskillNavigation");
            ModelState.Remove("IdtalentoNavigation");

            if (ModelState.IsValid)
            {
                // Verificar se o talento já tem esta skill
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
        
        // GET: Adicionar Experiência
        public async Task<IActionResult> AdicionarExperiencia(int id)
        {
            var talento = await _context.Talentos.FindAsync(id);
            if (talento == null) return NotFound();

            var model = new Experiencia { Idtalento = id };
            return View(model);
        }

// POST: Adicionar Experiência
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarExperiencia(Experiencia exp)
        {
            ModelState.Remove("IdtalentoNavigation");

            if (ModelState.IsValid)
            {
                // --- VALIDAÇÃO DE SOBREPOSIÇÃO (Requisito 6) ---
                // Verificar se existe alguma experiência que se sobreponha ao ano de início ou fim da nova experiência
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