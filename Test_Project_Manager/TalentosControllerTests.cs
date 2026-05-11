using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Controllers;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using System.Security.Claims;

namespace Test_Project_Manager;

public class TalentosControllerTests
{
    private static ClaimsPrincipal CriarUser(string userId)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "TestAuth"));
    }

    [Fact]
    public async Task Details_IdNulo_DeveRetornarNotFound()
    {
        var talentoMock = new Mock<ITalentoService>();
        var categoriaMock = new Mock<IService<Categoriatalento>>();
        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        var result = await controller.Details(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_TalentoNaoExiste_DeveRetornarNotFound()
    {
        var talentoMock = new Mock<ITalentoService>();
        talentoMock.Setup(s => s.GetDetalhesCompletosAsync(1))
                   .ReturnsAsync((Talento?)null);

        var categoriaMock = new Mock<IService<Categoriatalento>>();
        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        var result = await controller.Details(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_TalentoExiste_DeveRetornarView()
    {
        var talento = new Talento
        {
            Idtalento = 1,
            Idutilizador = 1
        };

        var talentoMock = new Mock<ITalentoService>();
        talentoMock.Setup(s => s.GetDetalhesCompletosAsync(1))
                   .ReturnsAsync(talento);

        var categoriaMock = new Mock<IService<Categoriatalento>>();
        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        var result = await controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(talento, viewResult.Model);
    }

    [Fact]
    public async Task Edit_IdNulo_DeveRetornarNotFound()
    {
        var talentoMock = new Mock<ITalentoService>();
        var categoriaMock = new Mock<IService<Categoriatalento>>();
        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        var result = await controller.Edit(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_TalentoDeOutroUtilizador_DeveRetornarNotFound()
    {
        var talento = new Talento
        {
            Idtalento = 1,
            Idutilizador = 2,
            Idcategoria = 1
        };

        var talentoMock = new Mock<ITalentoService>();
        talentoMock.Setup(s => s.GetDetalhesCompletosAsync(1))
                   .ReturnsAsync(talento);

        var categoriaMock = new Mock<IService<Categoriatalento>>();
        categoriaMock.Setup(s => s.GetAllAsync())
                     .ReturnsAsync(new List<Categoriatalento>());

        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CriarUser("1") }
        };

        var result = await controller.Edit(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ModelValido_DeveRedirecionarParaDetails()
    {
        var talento = new Talento
        {
            Idtalento = 1,
            Idcategoria = 1
        };

        var talentoMock = new Mock<ITalentoService>();
        talentoMock.Setup(s => s.CriarTalentoAsync(talento, 1))
                   .ReturnsAsync(10);

        var categoriaMock = new Mock<IService<Categoriatalento>>();
        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CriarUser("1") }
        };

        var result = await controller.Create(talento);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Details", redirect.ActionName);
        Assert.Equal(10, redirect.RouteValues!["id"]);
    }

    [Fact]
    public async Task AdicionarExperiencia_Get_DeveRetornarViewComModelo()
    {
        var talentoMock = new Mock<ITalentoService>();
        var categoriaMock = new Mock<IService<Categoriatalento>>();
        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        var result = controller.AdicionarExperiencia(5);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Experiencia>(viewResult.Model);
        Assert.Equal(5, model.Idtalento);
    }

    [Fact]
    public async Task DeleteConfirmed_DeveRedirecionarParaIndex()
    {
        var talentoMock = new Mock<ITalentoService>();
        var categoriaMock = new Mock<IService<Categoriatalento>>();
        var skillMock = new Mock<IService<Skill>>();

        var controller = new TalentosController(
            talentoMock.Object,
            categoriaMock.Object,
            skillMock.Object);

        var result = await controller.DeleteConfirmed(1);

        talentoMock.Verify(s => s.EliminarTalentoAsync(1), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }
}