using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace ProfileMAnager.Services
{
    public class RelatorioSkillStrategy : IRelatorioStrategy
    {
        private readonly AppDbContext _context;

        public RelatorioSkillStrategy(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RelatorioCategoriaPaisVM>> GerarRelatorio(
            string? categoria,
            string? pais,
            string? skill)
        {
            var query = from ts in _context.Talentoskills
                join s in _context.Skills
                    on ts.Idskill equals s.Idskill

                join t in _context.Talentos
                    on ts.Idtalento equals t.Idtalento

                join c in _context.Categoriatalentos
                    on t.Idcategoria equals c.Idcategoria

                select new RelatorioCategoriaPaisVM
                {
                    Categoria = c.Nome,
                    Pais = t.Pais,
                    Skill = s.Nome,
                    Total = 1,
                    PrecoMedioMensal = t.Precohora * 176
                };

            if (!string.IsNullOrEmpty(skill))
            {
                query = query.Where(x => x.Skill == skill);
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(x => x.Categoria == categoria);
            }

            if (!string.IsNullOrEmpty(pais))
            {
                query = query.Where(x => x.Pais == pais);
            }

            return await query.ToListAsync();
        }
    }
}