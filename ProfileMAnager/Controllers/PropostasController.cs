using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using ProfileMAnager.Services; 
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProfileMAnager.Controllers
{
    [Authorize]
    public class PropostasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPropostaService _propostaService;
        private readonly IService<Cliente> _clienteRepo; 
        private readonly IService<Categoriatalento> _categoriaRepo; 

        // Injeção de Dependência
        public PropostasController(AppDbContext context, IPropostaService propostaService)
        {
            _context = context;
            _propostaService = propostaService;
        }

        
        private async Task CarregarDadosFormulario()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                ViewBag.Clientes = await _context.Clientes.Where(c => c.Idutilizador == userId).ToListAsync();
            }
            else
            {
                ViewBag.Clientes = new List<Cliente>();
            }
            ViewBag.Categorias = await _context.Categoriatalentos.ToListAsync();
            ViewBag.Skills = await _context.Skills.ToListAsync();
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var propostas = await _propostaService.GetPropostasPorUtilizadorAsync(userId);
            return View(propostas);
        }

        public async Task<IActionResult> Create()
        {
            await CarregarDadosFormulario();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Propostatrabalho proposta, int[] selectedSkills, int[] anosExperiencia)
        {
            ModelState.Remove("IdclienteNavigation");
            ModelState.Remove("IdcategoriaNavigation");
            ModelState.Remove("IdutilizadorNavigation");

            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
                if (userIdClaim != null)
                {
                    proposta.Idutilizador = int.Parse(userIdClaim);
                    
                    await _propostaService.CriarPropostaAsync(proposta, selectedSkills, anosExperiencia);
                    return RedirectToAction(nameof(Index));
                }
            }

            // Se falha, volta a carregar
            ViewBag.Idcliente = new SelectList(await _clienteRepo.GetAllAsync(), "Idcliente", "Nome", proposta.Idcliente);
            ViewBag.Idcategoria = new SelectList(await _categoriaRepo.GetAllAsync(), "Idcategoria", "Nome", proposta.Idcategoria);
            return View(proposta);
        }  
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            
            var proposta = await _propostaService.GetPropostaDetalhadaAsync(id.Value);
            if (proposta == null) return NotFound();

            await CarregarDadosFormulario();
            return View(proposta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Propostatrabalho proposta, int[] selectedSkills, int[] anosMinimos)
        {
            if (id != proposta.Idproposta) return NotFound();
            
            ModelState.Remove("IdclienteNavigation");
            ModelState.Remove("IdcategoriaNavigation");
            ModelState.Remove("IdutilizadorNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    await _propostaService.AtualizarPropostaAsync(id, proposta, selectedSkills, anosMinimos);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao atualizar: " + ex.Message);
                }
            }
            await CarregarDadosFormulario();
            return View(proposta);
        }

        // MATCHING
        public async Task<IActionResult> Matching(int id)
        {
            var proposta = await _propostaService.GetPropostaDetalhadaAsync(id);
    
            if (proposta == null) return NotFound();

            var listaTalentos = await _propostaService.GetMatchingTalentosAsync(id);
            var modelo = listaTalentos.ToList();

            ViewBag.Proposta = proposta;
            
            return View("Matching", modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtribuirTalento(int idProposta, int idTalento)
        {
            bool sucesso = await _propostaService.AtribuirTalentoAsync(idProposta, idTalento);
            
            if (!sucesso) return RedirectToAction("Matching", new { id = idProposta });
            
            return RedirectToAction("Edit", new { id = idProposta });
        }

        // GET: Propostas/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var proposta = await _propostaService.GetDetalhesAsync(id.Value);

            if (proposta == null) return NotFound();
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                if (proposta.Idutilizador != userId) return Forbid();
            }

            // Resolve o erro de "Ambiguous invocation"
            return View(model: proposta);
        }

        // POST: Propostas/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _propostaService.EliminarPropostaAsync(id);
    
            TempData["Sucesso"] = "Proposta eliminada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarEstado(int id, string estado)
        {
            await _propostaService.AlterarEstadoTalentoAsync(id, estado);
            
            var pt = await _context.PropostaTalentos.FindAsync(id);
            return RedirectToAction("Edit", new { id = pt?.Idproposta });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverTalento(int id)
        {
            var pt = await _context.PropostaTalentos.FindAsync(id);
            int? propostaId = pt?.Idproposta;

            await _propostaService.RemoverTalentoDaPropostaAsync(id);

            return RedirectToAction("Edit", new { id = propostaId });
        }
    }
}