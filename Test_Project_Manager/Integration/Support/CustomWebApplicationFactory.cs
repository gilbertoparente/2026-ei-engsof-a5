using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProfileMAnager.Data;
using ProfileMAnager.Models;

namespace Test_Project_Manager.Integration.Support;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveDbContext<AppDbContext>(services);
            RemoveDbContext<GerirProposta>(services);

            var appDbName = $"ProfileManagerIntegration-{Guid.NewGuid()}";
            var gerirPropostaDbName = $"GerirPropostaIntegration-{Guid.NewGuid()}";

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(appDbName));

            services.AddDbContext<GerirProposta>(options =>
                options.UseInMemoryDatabase(gerirPropostaDbName));

            services.PostConfigure<CookieAuthenticationOptions>(
                CookieAuthenticationDefaults.AuthenticationScheme,
                options => options.Cookie.SecurePolicy = CookieSecurePolicy.None);
        });
    }

    private static void RemoveDbContext<TContext>(IServiceCollection services)
        where TContext : DbContext
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<TContext>));

        if (descriptor != null)
            services.Remove(descriptor);
    }
}
