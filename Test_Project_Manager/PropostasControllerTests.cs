using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Controllers;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using System.Security.Claims;

namespace Test_Project_Manager;

public class PropostasControllerTests
{
    private static ClaimsPrincipal CriarUser(string userId)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "TestAuth"));
    }

    private static AppDbContext CriarContextoFake()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Index_SemUserIdValido_DeveRetornarUnauthorized()
    {
        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();

        var controller = new PropostasController(context, propostaMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = await controller.Index();

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Index_ComUserValido_DeveRetornarView()
    {
        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();
        propostaMock.Setup(s => s.GetPropostasPorUtilizadorAsync(1))
                    .ReturnsAsync(new List<Propostatrabalho>());

        var controller = new PropostasController(context, propostaMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CriarUser("1") }
        };

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.Model);
    }

    [Fact]
    public async Task Delete_IdNulo_DeveRetornarNotFound()
    {
        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();

        var controller = new PropostasController(context, propostaMock.Object);

        var result = await controller.Delete(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_PropostaNaoExiste_DeveRetornarNotFound()
    {
        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();
        propostaMock.Setup(s => s.GetDetalhesAsync(1))
                    .ReturnsAsync((Propostatrabalho?)null);

        var controller = new PropostasController(context, propostaMock.Object);

        var result = await controller.Delete(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_PropostaDeOutroUtilizador_DeveRetornarForbid()
    {
        var proposta = new Propostatrabalho
        {
            Idproposta = 1,
            Idutilizador = 2
        };

        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();
        propostaMock.Setup(s => s.GetDetalhesAsync(1))
                    .ReturnsAsync(proposta);

        var controller = new PropostasController(context, propostaMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CriarUser("1") }
        };

        var result = await controller.Delete(1);

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task Matching_PropostaNaoExiste_DeveRetornarNotFound()
    {
        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();
        propostaMock.Setup(s => s.GetPropostaDetalhadaAsync(1))
                    .ReturnsAsync((Propostatrabalho?)null);

        var controller = new PropostasController(context, propostaMock.Object);

        var result = await controller.Matching(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AtribuirTalento_SemSucesso_DeveRedirecionarParaMatching()
    {
        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();
        propostaMock.Setup(s => s.AtribuirTalentoAsync(1, 2))
                    .ReturnsAsync(false);

        var controller = new PropostasController(context, propostaMock.Object);

        var result = await controller.AtribuirTalento(1, 2);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Matching", redirect.ActionName);
    }

    [Fact]
    public async Task AtribuirTalento_ComSucesso_DeveRedirecionarParaEdit()
    {
        var context = CriarContextoFake();
        var propostaMock = new Mock<IPropostaService>();
        propostaMock.Setup(s => s.AtribuirTalentoAsync(1, 2))
                    .ReturnsAsync(true);

        var controller = new PropostasController(context, propostaMock.Object);

        var result = await controller.AtribuirTalento(1, 2);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Edit", redirect.ActionName);
    }
}