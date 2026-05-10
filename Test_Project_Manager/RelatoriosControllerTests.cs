using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Controllers;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using Xunit;

namespace Test_Project_Manager
{
    public class RelatoriosControllerTests
    {
        private AppDbContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task RelatorioCategoriaPais_DeveRetornarViewComResultado()
        {
            var context = CriarContexto();

            context.Categoriatalentos.AddRange(
                new Categoriatalento { Idcategoria = 1, Nome = "Backend" },
                new Categoriatalento { Idcategoria = 2, Nome = "Frontend" }
            );

            context.Talentos.AddRange(
                new Talento { Idtalento = 1, Nome = "João", Pais = "Portugal" },
                new Talento { Idtalento = 2, Nome = "Carlos", Pais = "Espanha" }
            );

            context.Skills.AddRange(
                new Skill { Idskill = 1, Nome = "C#" },
                new Skill { Idskill = 2, Nome = "SQL" }
            );

            await context.SaveChangesAsync();

            var relatorioService = new RelatorioService(context);
            var controller = new RelatoriosController(relatorioService, context);

            var result = await controller.RelatorioCategoriaPais("Backend", "Portugal", "C#");

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.NotNull(controller.ViewBag.Categorias);
            Assert.NotNull(controller.ViewBag.Paises);
            Assert.NotNull(controller.ViewBag.Skills);
        }
    }
}