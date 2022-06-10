using DiscordApi.Data;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlite("Data Source=Database.db"));

builder.Services.AddSingleton<IStateHandler, StateHandler>();
builder.Services.AddSingleton<BotHostService>();
builder.Services.AddHostedService(p => p.GetRequiredService<BotHostService>());

// add exception logging
builder.Services.AddMvc(options => { options.Filters.Add(new SerilogExceptionLogger()); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Trace()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDefaultFiles();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".vue"] = "application/javascript";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.MapControllers();

app.Run();