using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Controllers;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using System.Security.Claims;

namespace Test_Project_Manager;

public class ClientesControllerTests
{
    private static ClaimsPrincipal CriarUser(string userId)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "TestAuth"));
    }

    [Fact]
    public async Task Edit_IdNulo_DeveRetornarNotFound()
    {
        var serviceMock = new Mock<IClienteService>();
        var controller = new ClientesController(serviceMock.Object);

        var result = await controller.Edit(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ClienteNaoExiste_DeveRetornarNotFound()
    {
        var serviceMock = new Mock<IClienteService>();
        serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Cliente?)null);

        var controller = new ClientesController(serviceMock.Object);

        var result = await controller.Edit(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ClienteDeOutroUtilizador_DeveRetornarForbid()
    {
        var cliente = new Cliente
        {
            Idcliente = 1,
            Idutilizador = 2,
            Nome = "Cliente Teste"
        };

        var serviceMock = new Mock<IClienteService>();
        serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(cliente);

        var controller = new ClientesController(serviceMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CriarUser("1") }
        };

        var result = await controller.Edit(1);

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task Create_ModelValido_DeveRedirecionarParaIndex()
    {
        var serviceMock = new Mock<IClienteService>();
        var controller = new ClientesController(serviceMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = CriarUser("1") }
        };

        var cliente = new Cliente
        {
            Nome = "Cliente A"
        };

        var result = await controller.Create(cliente);

        serviceMock.Verify(s => s.AddAsync(cliente), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }
}