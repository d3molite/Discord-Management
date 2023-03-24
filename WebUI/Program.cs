using BotModule.DI;
using BotModule.Extensions.Logging;
using DB;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProcessProvider;
using Serilog;
using WebUI.Areas.Identity;
using WebUI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlite("Data Source=ApiDb.db"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddSingleton<IModuleState, ModuleState>();
builder.Services.AddSingleton<ILanguageProvider, LanguageProvider>();
builder.Services.AddSingleton<ILoggingProvider, LoggingProvider>();
builder.Services.AddSingleton<ProcessProviderService>();
builder.Services.AddHostedService(p => p.GetRequiredService<ProcessProviderService>());

Log.Logger = new LoggerConfiguration()
    .WriteTo.Trace()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();