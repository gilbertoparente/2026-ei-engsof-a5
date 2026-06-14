using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Controllers;
using ProfileMAnager.Data;
using ProfileMAnager.Models;
using ProfileMAnager.Models.ViewModels;
using ProfileMAnager.Services;
using System.Security.Claims;

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

    [Fact]
    public async Task Perfil_ComNovaPassword_DeveGuardarHashBCrypt()
    {
        var authMock = new Mock<IAutenticacaoService>();
        using var context = CriarContexto();
        var utilizador = new Utilizador
        {
            Idutilizador = 1,
            Nome = "Ruben",
            Email = "teste@email.com",
            Passwordhash = BCrypt.Net.BCrypt.HashPassword("password-antiga")
        };
        context.Utilizadors.Add(utilizador);
        await context.SaveChangesAsync();

        var controller = new ContaController(authMock.Object, context);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, utilizador.Idutilizador.ToString())
                }))
            }
        };
        controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

        var model = new PerfilViewModel
        {
            Nome = "Ruben Atualizado",
            Email = "teste@email.com",
            NovaPassword = "password-nova",
            ConfirmarPassword = "password-nova"
        };

        var result = await controller.Perfil(model);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Perfil", redirectResult.ActionName);
        Assert.NotEqual("password-nova", utilizador.Passwordhash);
        Assert.True(BCrypt.Net.BCrypt.Verify("password-nova", utilizador.Passwordhash));
    }

    [Fact]
    public void Autenticar_ComPasswordGuardadaEmTexto_DeveRepararHashEManterUtilizador()
    {
        using var context = CriarContexto();
        var utilizador = new Utilizador
        {
            Idutilizador = 1,
            Nome = "Ruben",
            Email = "teste@email.com",
            Passwordhash = "password-nova"
        };
        context.Utilizadors.Add(utilizador);
        context.SaveChanges();

        var service = new ServicoAutenticacao(context);

        var autenticado = service.Autenticar("teste@email.com", "password-nova");

        Assert.NotNull(autenticado);
        Assert.Equal(utilizador.Idutilizador, autenticado.Idutilizador);
        Assert.NotEqual("password-nova", utilizador.Passwordhash);
        Assert.True(BCrypt.Net.BCrypt.Verify("password-nova", utilizador.Passwordhash));
    }
}
