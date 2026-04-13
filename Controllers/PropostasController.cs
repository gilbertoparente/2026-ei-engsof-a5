using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProfileMAnager.Data;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class PropostasController : Controller
    {
        private readonly AppDbContext _context;
        public PropostasController(AppDbContext context)
        {
            _context = context;
        }

        // lista
        public async Task<IActionResult> Index()
        {
            var propostas = await _context.Propostatrabalhos
                .Include(p => p.IdclienteNavigation)
                .Include(p => p.IdcategoriaNavigation)
                .ToListAsync();

            return View(propostas);
        }

        // criar
        public IActionResult Create()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Clientes = _context.Clientes.Where(c => c.Idutilizador == userId).ToList();
            ViewBag.Categorias = _context.Categoriatalentos.ToList();
            ViewBag.Skills = _context.Skills.ToList();
            return View();
        }

        // criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Propostatrabalho proposta, int[] selectedSkills, int[] anosMinimos)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            proposta.Idutilizador = userId;
            proposta.CreatedAt = DateTime.UtcNow;
            proposta.UpdatedAt = DateTime.UtcNow;
            proposta.Estado = "ABERTA";
            
            _context.Propostatrabalhos.Add(proposta);
            await _context.SaveChangesAsync();

            if (selectedSkills != null)
            {
                for (int i = 0; i < selectedSkills.Length; i++)
                {
                    _context.Propostaskills.Add(new Propostaskill
                    {
                        Idproposta = proposta.Idproposta,
                        Idskill = selectedSkills[i],
                        Anosminimosexperiencia = anosMinimos[i]
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proposta = await _context.Propostatrabalhos
                .Include(p => p.PropostaTalentos)
                    .ThenInclude(pt => pt.Talento)
                .Include(p => p.Propostaskills)
                .FirstOrDefaultAsync(p => p.Idproposta == id);

            if (proposta == null) return NotFound();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Clientes = _context.Clientes.Where(c => c.Idutilizador == userId).ToList();
            ViewBag.Categorias = _context.Categoriatalentos.ToList();
            ViewBag.Skills = _context.Skills.ToList();

            return View(proposta);
        }

        //editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Propostatrabalho proposta)
        {
            if (id != proposta.Idproposta) return NotFound();

            if (ModelState.IsValid)
            {
                try 
                {
                    var propostaDb = await _context.Propostatrabalhos.FindAsync(id);
                    if (propostaDb == null) return NotFound();

                   propostaDb.Nome = proposta.Nome;
                    propostaDb.Descricao = proposta.Descricao;
                    propostaDb.Idcliente = proposta.Idcliente;
                    propostaDb.Idcategoria = proposta.Idcategoria;
                    propostaDb.Horastotais = proposta.Horastotais;
                    propostaDb.Estado = proposta.Estado; 
                    propostaDb.UpdatedAt = DateTime.UtcNow;

                    _context.Update(propostaDb);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) 
                { 
                    ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
                }
            }

           
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Clientes = _context.Clientes.Where(c => c.Idutilizador == userId).ToList();
            ViewBag.Categorias = _context.Categoriatalentos.ToList();
            ViewBag.Skills = _context.Skills.ToList();
    
            return View(proposta);
        }

        // deletes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var proposta = await _context.Propostatrabalhos
                .Include(p => p.Propostaskills)
                .Include(p => p.PropostaTalentos)
                .FirstOrDefaultAsync(p => p.Idproposta == id);

            if (proposta != null)
            {
                _context.Propostaskills.RemoveRange(proposta.Propostaskills);
                _context.PropostaTalentos.RemoveRange(proposta.PropostaTalentos);
                _context.Propostatrabalhos.Remove(proposta);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // altera o estado
        [HttpPost]
        public async Task<IActionResult> AlterarEstado(int id, string estado)
        {
            var pt = await _context.PropostaTalentos.FindAsync(id);

            if (pt == null) return NotFound();

            pt.Estado = estado;

            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = pt.Idproposta });
        }
        
        public async Task<IActionResult> Matching(int id)
        {
            var proposta = await _context.Propostatrabalhos
                .Include(p => p.Propostaskills)
                .ThenInclude(ps => ps.IdskillNavigation)
                .FirstOrDefaultAsync(p => p.Idproposta == id);

            if (proposta == null)
                return NotFound();

            var talentos = await _context.Talentos
                .Include(t => t.Talentoskills)
                .ThenInclude(ts => ts.IdskillNavigation)
                .Where(t => t.Publico == true)
                .ToListAsync();

           var talentosElegiveis = talentos.Where(t =>
                proposta.Propostaskills.All(req =>
                    t.Talentoskills.Any(ts =>
                        ts.Idskill == req.Idskill &&
                        (ts.Anosexperiencia ?? 0) >= (req.Anosminimosexperiencia ?? 0)
                    )
                )
            ).ToList();

           talentosElegiveis = talentosElegiveis
                .OrderBy(t => t.Precohora * (proposta.Horastotais ?? 0))
                .ToList();

            ViewBag.Proposta = proposta;

            return View(talentosElegiveis);
        }
        // atrribui um talento
        [HttpPost]
        public async Task<IActionResult> AtribuirTalento(int idProposta, int idTalento)
        {
           var jaExiste = await _context.PropostaTalentos
                .AnyAsync(pt => pt.Idproposta == idProposta && pt.Idtalento == idTalento);

            if (jaExiste)
            {
               return RedirectToAction("Matching", new { id = idProposta });
            }

            var novaAssociacao = new PropostaTalento
            {
                Idproposta = idProposta,
                Idtalento = idTalento,
                Datainicio = DateTime.UtcNow, 
                Estado = "Pendente"
            };

            _context.PropostaTalentos.Add(novaAssociacao);
            await _context.SaveChangesAsync();
    
            return RedirectToAction("Index"); 
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoverTalento(int id)
        {
            var pt = await _context.PropostaTalentos.FindAsync(id);
            if (pt != null)
            {
                _context.PropostaTalentos.Remove(pt);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Edit", new { id = pt?.Idproposta });
        }
            
    }
}