using Microsoft.AspNetCore.Mvc.Testing;

namespace Test_Project_Manager.Integration.Support;

public static class AuthenticatedClientHelper
{
    public static async Task<HttpClient> CreateAuthenticatedClientAsync(
        CustomWebApplicationFactory factory,
        string? email = null,
        string password = "Password123!")
    {
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        email ??= $"integration-{Guid.NewGuid():N}@teste.local";

        var registerResponse = await IntegrationFormHelper.PostFormAsync(
            client,
            "/Conta/Registo",
            "/Conta/Registo",
            new Dictionary<string, string>
            {
                ["nome"] = "Utilizador Integracao",
                ["email"] = email,
                ["password"] = password
            });

        Assert.Equal(System.Net.HttpStatusCode.Redirect, registerResponse.StatusCode);

        var loginResponse = await IntegrationFormHelper.PostFormAsync(
            client,
            "/Conta/Login",
            "/Conta/Login",
            new Dictionary<string, string>
            {
                ["email"] = email,
                ["password"] = password
            });

        Assert.Equal(System.Net.HttpStatusCode.Redirect, loginResponse.StatusCode);

        return client;
    }
}
