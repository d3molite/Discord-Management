using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using DiscordApi.Data;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlite("Data Source=Database.db"));

builder.Services.AddSingleton<IStateHandler, StateHandler>();
builder.Services.AddSingleton<BotHostService>();
builder.Services.AddHostedService(p => p.GetRequiredService<BotHostService>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Trace()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();