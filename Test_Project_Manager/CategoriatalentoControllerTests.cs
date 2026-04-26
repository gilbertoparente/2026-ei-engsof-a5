using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Controllers;
using ProfileMAnager.Models;
using ProfileMAnager.Services;

namespace Test_Project_Manager;

public class CategoriatalentoControllerTests
{
    [Fact]
    public async Task Edit_IdNulo_DeveRetornarNotFound()
    {
        var repoMock = new Mock<IService<Categoriatalento>>();
        var controller = new CategoriatalentoController(repoMock.Object);

        var result = await controller.Edit(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_CategoriaNaoExiste_DeveRetornarNotFound()
    {
        var repoMock = new Mock<IService<Categoriatalento>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Categoriatalento?)null);

        var controller = new CategoriatalentoController(repoMock.Object);

        var result = await controller.Edit(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_CategoriaExiste_DeveRetornarViewComModelo()
    {
        var categoria = new Categoriatalento
        {
            Idcategoria = 1,
            Nome = "Backend"
        };

        var repoMock = new Mock<IService<Categoriatalento>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(categoria);

        var controller = new CategoriatalentoController(repoMock.Object);

        var result = await controller.Edit(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Categoriatalento>(viewResult.Model);
        Assert.Equal(1, model.Idcategoria);
    }

    [Fact]
    public async Task Create_ModelValido_DeveRedirecionarParaIndex()
    {
        var categoria = new Categoriatalento
        {
            Idcategoria = 1,
            Nome = "Frontend"
        };

        var repoMock = new Mock<IService<Categoriatalento>>();
        var controller = new CategoriatalentoController(repoMock.Object);

        var result = await controller.Create(categoria);

        repoMock.Verify(r => r.AddAsync(categoria), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }

    [Fact]
    public async Task Edit_IdDiferenteDoModelo_DeveRetornarNotFound()
    {
        var categoria = new Categoriatalento
        {
            Idcategoria = 2,
            Nome = "DevOps"
        };

        var repoMock = new Mock<IService<Categoriatalento>>();
        var controller = new CategoriatalentoController(repoMock.Object);

        var result = await controller.Edit(1, categoria);

        Assert.IsType<NotFoundResult>(result);
    }
}