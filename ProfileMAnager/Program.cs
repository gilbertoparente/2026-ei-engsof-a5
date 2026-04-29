using Microsoft.EntityFrameworkCore;
using ProfileMAnager.Data;
using ProfileMAnager.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configuração base
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Serviços Gerais
builder.Services.AddScoped<IPropostaService, PropostaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped(typeof(IService<>), typeof(Repository<>));
builder.Services.AddScoped<IAutenticacaoService, ServicoAutenticacao>();
builder.Services.AddScoped<IPesquisaService, PesquisaService>();
builder.Services.AddScoped<ITalentoService, TalentoService>();
builder.Services.AddScoped<RelatorioService>();

// --- CONFIGURAÇÃO DO PROXY (O Coração do teu vídeo) ---
// Registamos a classe real
builder.Services.AddScoped<DashboardService>(); 
// Registamos a Interface apontando para o Proxy que recebe a classe real
builder.Services.AddScoped<IDashboardService>(provider => 
    new DashboardServiceProxy(
        provider.GetRequiredService<DashboardService>(), 
        provider.GetRequiredService<IHttpContextAccessor>()
    )
);
// -------------------------------------------------------

// Bases de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ProfileMAnager.Models.GerirProposta>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticação
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Conta/Login";
        options.LogoutPath = "/Conta/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();