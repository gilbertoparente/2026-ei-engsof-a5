using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Test_Project_Manager.Integration.Support;

namespace Test_Project_Manager.Integration;

public class ContaIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ContaIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetLogin_DeveRetornarFormulario()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Conta/Login");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Login", html);
        Assert.Contains("name=\"email\"", html);
        Assert.Contains("name=\"password\"", html);
    }

    [Fact]
    public async Task GetPerfilSemAutenticacao_DeveRedirecionarParaLogin()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync("/Conta/Perfil");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("http://localhost/Conta/Login", response.Headers.Location?.ToString());
    }
}
