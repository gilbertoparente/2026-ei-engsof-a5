using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Controllers;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using ProfileMAnager.Services;

namespace Test_Project_Manager.Unit.Controllers;

public class ContaControllerTests
{
    private static AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void Login_Get_DeveRetornarView()
    {
        var authMock = new Mock<IAutenticacaoService>();
        using var context = CriarContexto();
        var controller = new ContaController(authMock.Object, context);

        var result = controller.Login();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Registo_Get_DeveRetornarView()
    {
        var authMock = new Mock<IAutenticacaoService>();
        using var context = CriarContexto();
        var controller = new ContaController(authMock.Object, context);

        var result = controller.Registo();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Login_CredenciaisInvalidas_DeveRetornarView()
    {
        var authMock = new Mock<IAutenticacaoService>();
        authMock.Setup(a => a.Autenticar("teste@email.com", "1234"))
                .Returns((Utilizador?)null);

        using var context = CriarContexto();
        var controller = new ContaController(authMock.Object, context);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = await controller.Login("teste@email.com", "1234");

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public void Registo_CamposVazios_DeveRetornarViewComErro()
    {
        var authMock = new Mock<IAutenticacaoService>();
        using var context = CriarContexto();
        var controller = new ContaController(authMock.Object, context);

        var result = controller.Registo("", "", "");

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Todos os campos são obrigatórios.", controller.ViewBag.Erro);
    }

    [Fact]
    public void Registo_EmailJaExiste_DeveRetornarViewComErro()
    {
        var authMock = new Mock<IAutenticacaoService>();
        authMock.Setup(a => a.Registar("Ruben", "teste@email.com", "1234"))
                .Returns(false);

        using var context = CriarContexto();
        var controller = new ContaController(authMock.Object, context);

        var result = controller.Registo("Ruben", "teste@email.com", "1234");

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Este email já se encontra registado.", controller.ViewBag.Erro);
    }

    [Fact]
    public void Registo_DadosValidos_DeveRedirecionarParaLogin()
    {
        var authMock = new Mock<IAutenticacaoService>();
        authMock.Setup(a => a.Registar("Ruben", "teste@email.com", "1234"))
                .Returns(true);

        using var context = CriarContexto();
        var controller = new ContaController(authMock.Object, context);
        controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

        var result = controller.Registo("Ruben", "teste@email.com", "1234");

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }
}
