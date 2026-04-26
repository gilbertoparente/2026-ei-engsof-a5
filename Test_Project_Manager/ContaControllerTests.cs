using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Controllers;
using ProfileMAnager.Models;
using ProfileMAnager.Services;

namespace Test_Project_Manager;

public class ContaControllerTests
{
    [Fact]
    public void Login_Get_DeveRetornarView()
    {
        var authMock = new Mock<IAutenticacaoService>();
        var controller = new ContaController(authMock.Object);

        var result = controller.Login();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Registo_Get_DeveRetornarView()
    {
        var authMock = new Mock<IAutenticacaoService>();
        var controller = new ContaController(authMock.Object);

        var result = controller.Registo();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Login_CredenciaisInvalidas_DeveRetornarView()
    {
        var authMock = new Mock<IAutenticacaoService>();
        authMock.Setup(a => a.Autenticar("teste@email.com", "1234"))
                .Returns((Utilizador?)null);

        var controller = new ContaController(authMock.Object);
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
        var controller = new ContaController(authMock.Object);

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

        var controller = new ContaController(authMock.Object);

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

        var controller = new ContaController(authMock.Object);
        controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

        var result = controller.Registo("Ruben", "teste@email.com", "1234");

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }
}