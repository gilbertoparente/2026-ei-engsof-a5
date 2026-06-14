using System.Net;
using Test_Project_Manager.Integration.Support;

namespace Test_Project_Manager.Integration;

public class HomeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public HomeIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetHome_DeveRetornarLandingPageFormatada()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("ProfileManager", html);
        Assert.Contains("/lib/bootstrap/dist/css/bootstrap.min.css", html);
        Assert.Contains("/css/site.css", html);
    }

    [Fact]
    public async Task GetBootstrapCss_DeveServirAssetEstatico()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/lib/bootstrap/dist/css/bootstrap.min.css");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/css", response.Content.Headers.ContentType?.MediaType);
    }
}
