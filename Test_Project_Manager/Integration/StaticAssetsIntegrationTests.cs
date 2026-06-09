using System.Net;
using Test_Project_Manager.Integration.Support;

namespace Test_Project_Manager.Integration;

public class StaticAssetsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public StaticAssetsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public static IEnumerable<object[]> StaticAssets()
    {
        yield return new object[] { "/css/site.css", "text/css" };
        yield return new object[] { "/css/logo.png", "image/png" };
        yield return new object[] { "/lib/bootstrap/dist/css/bootstrap.min.css", "text/css" };
        yield return new object[] { "/lib/bootstrap/dist/js/bootstrap.bundle.min.js", "text/javascript" };
        yield return new object[] { "/lib/jquery/dist/jquery.min.js", "text/javascript" };
        yield return new object[] { "/lib/jquery-validation/dist/jquery.validate.min.js", "text/javascript" };
        yield return new object[] { "/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js", "text/javascript" };
    }

    [Theory]
    [MemberData(nameof(StaticAssets))]
    public async Task GetAssetEstatico_DeveSerServidoPelaAplicacao(string url, string mediaType)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(mediaType, response.Content.Headers.ContentType?.MediaType);
    }
}
