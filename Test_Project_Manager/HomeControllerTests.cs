using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Controllers;
using ProfileMAnager.Models.ViewModels;
using ProfileMAnager.Services;
using System.Security.Claims;

namespace Test_Project_Manager;

public class HomeControllerTests
{
    [Fact]
    public async Task Index_UserNaoAutenticado_DeveRetornarViewComVisitante()
    {
        var dashboardMock = new Mock<IDashboardService>();
        var controller = new HomeController(dashboardMock.Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<DashboardViewModel>(viewResult.Model);
        Assert.Equal("Visitante", model.NomeUtilizador);
    }

    [Fact]
    public async Task Index_UserAutenticado_DeveRetornarViewComDashboard()
    {
        var model = new DashboardViewModel
        {
            NomeUtilizador = ""
        };

        var dashboardMock = new Mock<IDashboardService>();
        dashboardMock.Setup(s => s.GetDashboardDataAsync(1))
            .ReturnsAsync(model);

        var controller = new HomeController(dashboardMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "Ruben")
        }, "TestAuth"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var resultModel = Assert.IsType<DashboardViewModel>(viewResult.Model);
        Assert.Equal("Ruben", resultModel.NomeUtilizador);
    }
}