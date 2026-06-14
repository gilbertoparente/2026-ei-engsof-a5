using System.Net;
using Test_Project_Manager.Integration.Support;

namespace Test_Project_Manager.Integration;

public class AuthenticatedPagesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthenticatedPagesIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public static IEnumerable<object[]> AuthenticatedPages()
    {
        yield return new object[] { "/", "Utilizador Integracao" };
        yield return new object[] { "/Conta/Perfil", "Utilizador Integracao" };
        yield return new object[] { "/Clientes", "Gestão de Clientes" };
        yield return new object[] { "/Clientes/Create", "Criar Cliente" };
        yield return new object[] { "/Talentos", "Os Meus Perfis de Talento" };
        yield return new object[] { "/Talentos/Create", "Criar" };
        yield return new object[] { "/Skills", "Biblioteca de Skills" };
        yield return new object[] { "/Skills/Create", "Skill" };
        yield return new object[] { "/Propostas", "Propostas" };
        yield return new object[] { "/Propostas/Create", "Proposta" };
        yield return new object[] { "/Relatorios/RelatorioCategoriaPais", "Relatório" };
    }

    [Theory]
    [MemberData(nameof(AuthenticatedPages))]
    public async Task GetPaginaAutenticada_DeveRetornarOk(string url, string expectedContent)
    {
        var client = await AuthenticatedClientHelper.CreateAuthenticatedClientAsync(_factory);

        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(expectedContent, html);
    }
}
