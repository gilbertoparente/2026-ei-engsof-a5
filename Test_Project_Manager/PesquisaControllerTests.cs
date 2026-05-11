using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ProfileMAnager.Controllers;
using ProfileMAnager.Models;
using ProfileMAnager.Services;

namespace Test_Project_Manager;

public class PesquisaControllerTests
{
    [Fact]
    public async Task Index_SemSkills_DeveRetornarViewComListaVazia()
    {
        var pesquisaMock = new Mock<IPesquisaService>();
        pesquisaMock.Setup(s => s.GetListaSkillsOrdenadaAsync())
            .ReturnsAsync(new List<Skill>());

        var controller = new PesquisaController(pesquisaMock.Object);

        var result = await controller.Index(Array.Empty<int>());

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.Model);
    }

    [Fact]
    public async Task Index_ComSkills_DeveRetornarViewComResultados()
    {
        var pesquisaMock = new Mock<IPesquisaService>();
        pesquisaMock.Setup(s => s.GetListaSkillsOrdenadaAsync())
            .ReturnsAsync(new List<Skill>());
        pesquisaMock.Setup(s => s.PesquisarTalentosPorSkillsAsync(It.IsAny<int[]>()))
            .ReturnsAsync(new List<Talento>());

        var controller = new PesquisaController(pesquisaMock.Object);

        var result = await controller.Index(new[] { 1, 2 });

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.Model);
    }
}