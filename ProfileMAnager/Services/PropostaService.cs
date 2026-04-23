using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public class PropostaService : IPropostaService
    {
        private readonly AppDbContext _context;
        public PropostaService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Propostatrabalho>> GetPropostasPorUtilizadorAsync(int userId)
        {
            return await _context.Propostatrabalhos
                .Include(p => p.IdclienteNavigation).Include(p => p.IdcategoriaNavigation)
                .Where(p => p.Idutilizador == userId).ToListAsync();
        }

        public async Task<Propostatrabalho?> GetPropostaDetalhadaAsync(int id)
        {
            return await _context.Propostatrabalhos
                .Include("PropostaTalentos.Talento").Include("Propostaskills.IdskillNavigation")
                .FirstOrDefaultAsync(p => p.Idproposta == id);
        }
        
        public async Task AlterarEstadoTalentoAsync(int id, string estado)
        {
            var pt = await _context.PropostaTalentos.FindAsync(id);
            if (pt != null)
            {
                pt.Estado = estado;
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<Propostatrabalho?> GetDetalhesAsync(int id)
        {
            return await _context.Propostatrabalhos
                .Include(p => p.IdclienteNavigation)   
                .Include(p => p.IdcategoriaNavigation)  
                .Include(p => p.Propostaskills)         
                .ThenInclude(ps => ps.IdskillNavigation) 
                .FirstOrDefaultAsync(p => p.Idproposta == id);
        }

        public async Task RemoverTalentoDaPropostaAsync(int id)
        {
            var pt = await _context.PropostaTalentos.FindAsync(id);
            if (pt != null)
            {
                _context.PropostaTalentos.Remove(pt);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CriarPropostaAsync(Propostatrabalho proposta, int[] skills, int[] anos)
        {
            
            if (proposta.Idutilizador <= 0)
            {
                throw new Exception("O utilizador não foi identificado. Verifique se está logado.");
            }
            
            proposta.CreatedAt = DateTime.Now; 
            proposta.Estado = "Aberta";

            _context.Propostatrabalhos.Add(proposta);
            await _context.SaveChangesAsync(); 
            
            if (skills != null && skills.Length > 0)
            {
                for (int i = 0; i < skills.Length; i++)
                {
                    _context.Propostaskills.Add(new Propostaskill {
                        Idproposta = proposta.Idproposta,
                        Idskill = skills[i],
                        Anosminimosexperiencia = (anos != null && i < anos.Length) ? anos[i] : 0
                    });
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Talento>> GetMatchingTalentosAsync(int propostaId)
        {
            var proposta = await _context.Propostatrabalhos
                .Include(p => p.Propostaskills)
                .FirstOrDefaultAsync(p => p.Idproposta == propostaId);

            if (proposta == null) return Enumerable.Empty<Talento>();

            var talentos = await _context.Talentos.Include("Talentoskills.IdskillNavigation")
                .Where(t => t.Publico == true).ToListAsync();

            return talentos.Where(t => proposta.Propostaskills.All(req =>
                t.Talentoskills.Any(ts => ts.Idskill == req.Idskill && (ts.Anosexperiencia ?? 0) >= (req.Anosminimosexperiencia ?? 0))
            )).OrderBy(t => t.Precohora * (proposta.Horastotais ?? 0));
        }
        
        public async Task AtualizarPropostaAsync(int id, Propostatrabalho proposta, int[] skills, int[] anos)
        {
            var propostaDb = await _context.Propostatrabalhos
                .Include(p => p.Propostaskills)
                .FirstOrDefaultAsync(p => p.Idproposta == id);

            if (propostaDb == null) return;
            
            propostaDb.Nome = proposta.Nome;
            propostaDb.Descricao = proposta.Descricao;
            propostaDb.Idcliente = proposta.Idcliente;
            propostaDb.Idcategoria = proposta.Idcategoria;
            propostaDb.Horastotais = proposta.Horastotais;
            propostaDb.Estado = proposta.Estado;
            propostaDb.UpdatedAt = DateTime.UtcNow;
            
            _context.Propostaskills.RemoveRange(propostaDb.Propostaskills);
            if (skills != null)
            {
                for (int i = 0; i < skills.Length; i++)
                {
                    _context.Propostaskills.Add(new Propostaskill
                    {
                        Idproposta = id,
                        Idskill = skills[i],
                        Anosminimosexperiencia = i < anos.Length ? anos[i] : 0
                    });
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task EliminarPropostaAsync(int id)
        {
            var proposta = await _context.Propostatrabalhos
                .Include(p => p.Propostaskills)
                .FirstOrDefaultAsync(p => p.Idproposta == id);

            if (proposta != null)
            {
                
                _context.Propostaskills.RemoveRange(proposta.Propostaskills);
                
                _context.Propostatrabalhos.Remove(proposta);
        
                await _context.SaveChangesAsync();
            }
        }
        
            public async Task<bool> AtribuirTalentoAsync(int idProposta, int idTalento)
            {
                var jaExiste = await _context.PropostaTalentos
                     .AnyAsync(pt => pt.Idproposta == idProposta && pt.Idtalento == idTalento);

                if (jaExiste) return false;

                _context.PropostaTalentos.Add(new PropostaTalento {
                    Idproposta = idProposta,
                    Idtalento = idTalento,
                    Datainicio = DateTime.UtcNow,
                    Estado = "Pendente"
                });

                await _context.SaveChangesAsync();
                return true;
            }
        }
    }