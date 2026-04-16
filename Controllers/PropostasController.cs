using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        // Método auxiliar para preencher dados de Clientes, Categorias e Skills nas Views
        private async Task CarregarDadosFormulario()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var clientes = await _context.Clientes.Where(c => c.Idutilizador == userId).ToListAsync();
                ViewBag.Clientes = clientes;
            }
            else
            {
                ViewBag.Clientes = new List<Cliente>();
            }
            
            ViewBag.Categorias = await _context.Categoriatalentos.ToListAsync();
            ViewBag.Skills = await _context.Skills.ToListAsync();
        }

        // Listagem de propostas 
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var propostas = await _context.Propostatrabalhos
                .Include(p => p.IdclienteNavigation)
                .Include(p => p.IdcategoriaNavigation)
                .Where(p => p.Idutilizador == userId) 
                .ToListAsync();

            return View(propostas);
        }

        // GET: Propostas/Create
        public async Task<IActionResult> Create()
        {
            await CarregarDadosFormulario();
            return View();
        }

        // POST: Propostas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Propostatrabalho proposta, int[] selectedSkills, int[] anosMinimos)
        {
            ModelState.Clear();
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId)) proposta.Idutilizador = userId;

            proposta.CreatedAt = DateTime.UtcNow;
            proposta.UpdatedAt = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    proposta.IdclienteNavigation = null!;
                    proposta.IdcategoriaNavigation = null!;
                    proposta.IdutilizadorNavigation = null!;

                    _context.Add(proposta);
                    await _context.SaveChangesAsync();

                    // Adicionar as competências (skills)
                    if (selectedSkills != null)
                    {
                        for (int i = 0; i < selectedSkills.Length; i++)
                        {
                            var propostaSkill = new Propostaskill
                            {
                                Idproposta = proposta.Idproposta,
                                Idskill = selectedSkills[i],
                                Anosminimosexperiencia = (anosMinimos != null && i < anosMinimos.Length) ? anosMinimos[i] : 0
                            };
                            _context.Propostaskills.Add(propostaSkill);
                        }
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao gravar a proposta: " + ex.Message);
                }
            }
            await CarregarDadosFormulario();
            return View(proposta);
        }

        // GET: Propostas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var query = _context.Propostatrabalhos.AsQueryable();
           
            var proposta = await query
                .Include("PropostaTalentos.Talento") 
                .Include("Propostaskills.IdskillNavigation")
                .FirstOrDefaultAsync(p => p.Idproposta == id);

            if (proposta == null) return NotFound();

            await CarregarDadosFormulario();
            return View(proposta);
        }

        // POST: Propostas/Edit/5
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
                    ModelState.AddModelError("", "Erro ao atualizar a proposta: " + ex.Message);
                }
            }
            await CarregarDadosFormulario();
            return View(proposta);
        }

        // GET: Propostas/Matching
       public async Task<IActionResult> Matching(int id)
        {
            var proposta = await _context.Propostatrabalhos
                .Include("Propostaskills.IdskillNavigation")
                .FirstOrDefaultAsync(p => p.Idproposta == id);

            if (proposta == null) return NotFound();

            var talentos = await _context.Talentos
                .Include("Talentoskills.IdskillNavigation")
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

            ViewBag.Proposta = proposta;
            
            
            return View("Matching", (object)talentosElegiveis);
        }

        // POST: Associar um Talento a uma Proposta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtribuirTalento(int idProposta, int idTalento)
        {
            var jaExiste = await _context.PropostaTalentos
                 .AnyAsync(pt => pt.Idproposta == idProposta && pt.Idtalento == idTalento);

            if (jaExiste) return RedirectToAction("Matching", new { id = idProposta });

            var novaAssociacao = new PropostaTalento
            {
                Idproposta = idProposta,
                Idtalento = idTalento,
                Datainicio = DateTime.UtcNow,
                Estado = "Pendente"
            };

            _context.PropostaTalentos.Add(novaAssociacao);
            await _context.SaveChangesAsync();

      
            return RedirectToAction("Edit", new { id = idProposta });
        }

        // POST: Atualizar o estado (Candidato, Aceite, Rejeitado, etc)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarEstado(int id, string estado)
        {
            var pt = await _context.PropostaTalentos.FindAsync(id);
            if (pt == null) return NotFound();

            pt.Estado = estado;
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = pt.Idproposta });
        }

        // POST: Remover a associação entre Talento e Proposta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverTalento(int id)
        {
            var pt = await _context.PropostaTalentos.FindAsync(id);
            if (pt != null)
            {
                int idProposta = pt.Idproposta;
                _context.PropostaTalentos.Remove(pt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = idProposta });
            }
            return RedirectToAction("Index");
        }

        // POST: Eliminar a proposta e todas as suas dependências
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
    }
}