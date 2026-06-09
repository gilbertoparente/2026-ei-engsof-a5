using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProfileMAnager.Data;
using Test_Project_Manager.Integration.Support;

namespace Test_Project_Manager.Integration;

public class AuthenticationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthenticationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetRegisto_DeveRetornarFormulario()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Conta/Registo");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Criar Conta", html);
        Assert.Contains("name=\"nome\"", html);
        Assert.Contains("name=\"email\"", html);
        Assert.Contains("name=\"password\"", html);
    }

    [Fact]
    public async Task PostRegisto_DadosValidos_DeveCriarUtilizadorERedirecionarParaLogin()
    {
        var client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var email = $"registo-{Guid.NewGuid():N}@teste.local";

        var response = await IntegrationFormHelper.PostFormAsync(
            client,
            "/Conta/Registo",
            "/Conta/Registo",
            new Dictionary<string, string>
            {
                ["nome"] = "Teste Registo",
                ["email"] = email,
                ["password"] = "Password123!"
            });

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/Conta/Login", response.Headers.Location?.ToString());

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.True(await context.Utilizadors.AnyAsync(u => u.Email == email));
    }

    [Fact]
    public async Task PostLogin_CredenciaisValidas_DeveCriarCookieERedirecionarParaHome()
    {
        var client = await AuthenticatedClientHelper.CreateAuthenticatedClientAsync(_factory);

        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Utilizador Integracao", html);
    }

    [Fact]
    public async Task PostLogin_CredenciaisInvalidas_DeveRetornarLoginComErro()
    {
        var client = _factory.CreateClient();

        var response = await IntegrationFormHelper.PostFormAsync(
            client,
            "/Conta/Login",
            "/Conta/Login",
            new Dictionary<string, string>
            {
                ["email"] = "naoexiste@teste.local",
                ["password"] = "errada"
            });

        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Login", html);
        Assert.Contains("name=\"email\"", html);
    }
}
