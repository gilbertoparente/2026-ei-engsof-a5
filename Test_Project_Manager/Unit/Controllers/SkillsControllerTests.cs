using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileMAnager.Controllers;
using ProfileMAnager.Models;
using ProfileMAnager.Services;
using Xunit;

namespace Test_Project_Manager.Unit.Controllers
{
    public class SkillsControllerTests
    {
        [Fact]
        public async Task Create_Get_DeveRetornarView()
        {
            var skillRepoMock = new Mock<IService<Skill>>();
            var areaRepoMock = new Mock<IService<Areaprofissional>>();

            areaRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Areaprofissional>
                {
                    new Areaprofissional { Idarea = 1, Nome = "Software" }
                });

            var controller = new SkillsController(skillRepoMock.Object, areaRepoMock.Object);

            var result = await controller.Create();

            Assert.IsType<ViewResult>(result);
            Assert.NotNull(controller.ViewBag.Idarea);
        }

        [Fact]
        public async Task Create_Post_ModelValido_DeveRedirecionarParaIndex()
        {
            var skillRepoMock = new Mock<IService<Skill>>();
            var areaRepoMock = new Mock<IService<Areaprofissional>>();

            var controller = new SkillsController(skillRepoMock.Object, areaRepoMock.Object);

            var skill = new Skill
            {
                Idskill = 1,
                Nome = "C#",
                Idarea = 1
            };

            var result = await controller.Create(skill);

            skillRepoMock.Verify(r => r.AddAsync(skill), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Get_IdNulo_DeveRetornarNotFound()
        {
            var skillRepoMock = new Mock<IService<Skill>>();
            var areaRepoMock = new Mock<IService<Areaprofissional>>();

            var controller = new SkillsController(skillRepoMock.Object, areaRepoMock.Object);

            var result = await controller.Edit(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_SkillNaoExiste_DeveRetornarNotFound()
        {
            var skillRepoMock = new Mock<IService<Skill>>();
            var areaRepoMock = new Mock<IService<Areaprofissional>>();

            skillRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Skill?)null);

            var controller = new SkillsController(skillRepoMock.Object, areaRepoMock.Object);

            var result = await controller.Edit(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_SkillExiste_DeveRetornarView()
        {
            var skill = new Skill
            {
                Idskill = 1,
                Nome = "C#",
                Idarea = 1
            };

            var skillRepoMock = new Mock<IService<Skill>>();
            var areaRepoMock = new Mock<IService<Areaprofissional>>();

            skillRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(skill);

            areaRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Areaprofissional>
                {
                    new Areaprofissional { Idarea = 1, Nome = "Software" }
                });

            var controller = new SkillsController(skillRepoMock.Object, areaRepoMock.Object);

            var result = await controller.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(skill, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Post_IdDiferente_DeveRetornarNotFound()
        {
            var skillRepoMock = new Mock<IService<Skill>>();
            var areaRepoMock = new Mock<IService<Areaprofissional>>();

            var controller = new SkillsController(skillRepoMock.Object, areaRepoMock.Object);

            var skill = new Skill
            {
                Idskill = 2,
                Nome = "C#",
                Idarea = 1
            };

            var result = await controller.Edit(1, skill);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ModelValido_DeveRedirecionarParaIndex()
        {
            var skillRepoMock = new Mock<IService<Skill>>();
            var areaRepoMock = new Mock<IService<Areaprofissional>>();

            var controller = new SkillsController(skillRepoMock.Object, areaRepoMock.Object);

            var skill = new Skill
            {
                Idskill = 1,
                Nome = "C#",
                Idarea = 1
            };

            var result = await controller.Edit(1, skill);

            skillRepoMock.Verify(r => r.UpdateAsync(skill), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
    }
}
