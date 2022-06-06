using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApi.Data;
using DiscordApi.DiscordHost.Extensions.AntiSpam;
using DiscordApi.DiscordHost.Extensions.ESports;
using DiscordApi.DiscordHost.Extensions.ImageManipulation;
using DiscordApi.DiscordHost.Extensions.Interfaces;
using DiscordApi.DiscordHost.Extensions.Logging;
using DiscordApi.DiscordHost.Extensions.Modnotes;
using DiscordApi.DiscordHost.Extensions.ReactionRoles;
using DiscordApi.DiscordHost.Utils;
using Microsoft.EntityFrameworkCore;

namespace DiscordApi.DiscordHost.Bot;

public class DiscordBot
{
    private readonly DiscordSocketClient _client;

    private readonly DiscordSocketConfig _config = new()
        { MessageCacheSize = 100, GatewayIntents = GatewayIntents.All, UseInteractionSnowflakeDate = false };

    private readonly IStateHandler? _handler;

    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _provider;
    private readonly string _token;
    private AntiSpamExtension _antiSpam;

    private ILoggingExtension _logger;
    private ReactionRoleExtension _roleAssigner;

    public DiscordBot(string name, string token, IServiceProvider serviceProvider, string presence = "")
    {
        Name = name;
        _token = token;
        _provider = serviceProvider;
        _handler = serviceProvider.GetRequiredService<IStateHandler>();

        _client = new DiscordSocketClient(_config);
        _interactionService = new InteractionService(_client);

        if (!string.IsNullOrEmpty(presence))
            _client.SetGameAsync(presence);
        else
            _client.SetGameAsync("with Discord.NET");

        _client.Ready += Ready;
    }


    public string Name { get; set; }

    public string Status => _client.CurrentUser.Status.ToString();

    public string AvatarUrl => _client.CurrentUser.GetAvatarUrl();

    public List<string> Servers => _client.Guilds.Select(x => x.Name).ToList();

    private async Task LoadExtensions()
    {
        Serilog.Log.Information("Loading Extensions for {ClientName}", Name);
        await using var context = AppDBContext.Get();


        var configs = context.Configs
            .Include(prop => prop.AntiSpam)
            .Include(prop => prop.RelatedGuild)
            .Include(prop => prop.RelatedBot)
            .Include(prop => prop.RoleConfigs)
            .Where(p => p.RelatedBot.Name == Name).ToArray();

        foreach (var config in configs)
        {
            Serilog.Log.Debug("{ClientName}", config.RelatedBot.Name);
        }

        if (configs.Any())
        {
            if (configs.Any(p => p.RoleConfigs.Any())) _roleAssigner = new ReactionRoleExtension(Name, _client);
            if (configs.Any(p => p.AntiSpam != null)) _antiSpam = new AntiSpamExtension(_client, _logger, Name);
            if (configs.Any(p => p.ImageManipulationEnabled))
            {
                await _interactionService.AddModuleAsync<ImageCommandHandler>(_provider);
                Serilog.Log.Debug("Registered ImageManip to {ClientName}", Name);
            }

            if (configs.Any(p => p.ModnotesEnabled))
            {
                await _interactionService.AddModuleAsync<ModNoteCommandHandler>(_provider);
                Serilog.Log.Debug("Registered ModNotes to {ClientName}", Name);
            }

            if (configs.Any(p => p.ESportsEnabled))
            {
                await _interactionService.AddModuleAsync<ESportsCommandHandler>(_provider);
                Serilog.Log.Debug("Registered Esports to {ClientName}", Name);
            }
        }
    }

    public async Task StartBot()
    {
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        _logger = new GeneralLogger(_client, Name);

        await LoadExtensions();


        _client.Log += Log;

        await Task.Delay(-1);
    }

    private async Task Ready()
    {
        using var context = AppDBContext.Get();
        var configs = context.Configs
            .Include(prop => prop.RelatedGuild)
            .Where(p => p.RelatedBot.Name == Name);

        if (_interactionService.Modules.Any())
        {
            foreach (var config in configs)
            {
                Serilog.Log.Debug("Registering Commands for {ClientName} to {GuildName}", Name,
                    config.RelatedGuild.Name);
                await _interactionService.RegisterCommandsToGuildAsync(config.RelatedGuild.GuildID);
            }
        }
        else
        {
            Serilog.Log.Debug("{ClientName} has no modules.", Name);
        }

        _client.InteractionCreated += async interaction =>
        {
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(ctx, _provider);
        };
    }

    private Task Log(LogMessage msg)
    {
        Serilog.Log.Information("{BotName} {message}", Name, msg.ToString());
        return Task.CompletedTask;
    }
}