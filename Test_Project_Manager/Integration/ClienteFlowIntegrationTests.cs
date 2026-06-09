using System.Net;
using Test_Project_Manager.Integration.Support;

namespace Test_Project_Manager.Integration;

public class ClienteFlowIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ClienteFlowIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CriarClienteAutenticado_DeveRedirecionarEAparecerNaListagem()
    {
        var client = await AuthenticatedClientHelper.CreateAuthenticatedClientAsync(_factory);
        var nomeCliente = $"Cliente {Guid.NewGuid():N}";

        var createResponse = await IntegrationFormHelper.PostFormAsync(
            client,
            "/Clientes/Create",
            "/Clientes/Create",
            new Dictionary<string, string>
            {
                ["Nome"] = nomeCliente,
                ["Pais"] = "Portugal"
            });

        Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);
        Assert.Equal("/Clientes", createResponse.Headers.Location?.ToString());

        var indexResponse = await client.GetAsync("/Clientes");
        var html = await indexResponse.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, indexResponse.StatusCode);
        Assert.Contains(nomeCliente, html);
        Assert.Contains("Portugal", html);
    }
}
