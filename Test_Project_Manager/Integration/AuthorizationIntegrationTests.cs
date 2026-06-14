using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Test_Project_Manager.Integration.Support;

namespace Test_Project_Manager.Integration;

public class AuthorizationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthorizationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public static IEnumerable<object[]> ProtectedRoutes()
    {
        yield return new object[] { "/Conta/Perfil" };
        yield return new object[] { "/Clientes" };
        yield return new object[] { "/Clientes/Create" };
        yield return new object[] { "/Talentos" };
        yield return new object[] { "/Talentos/Create" };
        yield return new object[] { "/Skills" };
        yield return new object[] { "/Skills/Create" };
        yield return new object[] { "/Propostas" };
        yield return new object[] { "/Propostas/Create" };
        yield return new object[] { "/Relatorios/RelatorioCategoriaPais" };
    }

    [Theory]
    [MemberData(nameof(ProtectedRoutes))]
    public async Task GetRotaProtegidaSemLogin_DeveRedirecionarParaLogin(string url)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("http://localhost/Conta/Login", response.Headers.Location?.ToString());
    }
}
