using System.Net;
using System.Text.RegularExpressions;

namespace Test_Project_Manager.Integration.Support;

public static class IntegrationFormHelper
{
    private static readonly Regex AntiforgeryRegex = new(
        "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static async Task<HttpResponseMessage> PostFormAsync(
        HttpClient client,
        string getUrl,
        string postUrl,
        IDictionary<string, string> fields)
    {
        var getResponse = await client.GetAsync(getUrl);
        getResponse.EnsureSuccessStatusCode();

        var html = await getResponse.Content.ReadAsStringAsync();
        var token = ExtractAntiforgeryToken(html);

        var formFields = new Dictionary<string, string>(fields)
        {
            ["__RequestVerificationToken"] = token
        };

        return await client.PostAsync(postUrl, new FormUrlEncodedContent(formFields));
    }

    private static string ExtractAntiforgeryToken(string html)
    {
        var match = AntiforgeryRegex.Match(html);
        if (!match.Success)
            throw new InvalidOperationException("Antiforgery token not found in form.");

        return WebUtility.HtmlDecode(match.Groups[1].Value);
    }
}
