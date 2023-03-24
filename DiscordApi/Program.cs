using BotModule.DI;
using DB;
using DiscordApi.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using ProcessProvider;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlite("Data Source=Database.db"));
builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlite("Data Source=ApiDb.db"));

builder.Services.AddSingleton<IModuleState, ModuleState>();
builder.Services.AddSingleton<ILanguageProvider, LanguageProvider>();
builder.Services.AddSingleton<ProcessProviderService>();
builder.Services.AddHostedService(p => p.GetRequiredService<ProcessProviderService>());

// add exception logging
builder.Services.AddMvc(options => { options.Filters.Add(new SerilogExceptionLogger()); });

var app = builder.Build();

LocalizationService.Initialize();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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

if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDefaultFiles();

var provider = new FileExtensionContentTypeProvider
{
	Mappings =
	{
		[".vue"] = "application/javascript"
	}
};

app.UseStaticFiles(new StaticFileOptions
{
	ContentTypeProvider = provider
});

app.MapControllers();

app.Run();