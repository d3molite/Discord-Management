using BotModule.DI;
using BotModule.Extensions.Base;
using BotModule.Extensions.Feedback;
using BotModule.Extensions.Modnotes;
using BotModule.Extensions.VoiceChannels;
using DB.Models;
using DB.Repositories;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Strings.Util;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private readonly DiscordSocketClient _client;

    private readonly DiscordSocketConfig _config = new()
        { MessageCacheSize = 100, GatewayIntents = GatewayIntents.All, UseInteractionSnowflakeDate = false };

    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _serviceProvider;

    private Bot _botModel;

    public DiscordBot(Bot botModel, IServiceProvider serviceProvider)
    {
        _botModel = botModel;
        _serviceProvider = serviceProvider;
        _client = new DiscordSocketClient(_config);
        _interactionService = new InteractionService(_client);
    }

    public string Name => _botModel.Name;

    public async Task StartBot()
    {
        _client.Ready += async () =>
        {
            _botModel = BotRepository.UpdateSnowFlake(_botModel.Id, _client.CurrentUser.Id);

            if (_botModel.Configs == null)
            {
                Log.Debug("No Guild Configurations present for {ClientName}", _botModel.Name);
                return;
            }

            await CreateExtensions();
            await RegisterLanguages();
            await LoadModules();
            await UpdatePresence(_botModel.Presence);
        };

        await _client.LoginAsync(TokenType.Bot, _botModel.Token);
        await _client.StartAsync();
    }

    public async Task StopBot()
    {
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    private async Task LoadModules()
    {
        Log.Information("Loading Extensions for {ClientName}", _botModel.Name);

        foreach (var config in _botModel.Configs!)
        {
            var guild = _client.GetGuild(config.LinkedGuild.Snowflake);

            if (guild == null)
            {
                Log.Error("Could not fetch guild for Snowflake: {Snowflake}", config.LinkedGuild.Snowflake);
                continue;
            }

            if (config.FeedbackConfig != null)
                await LoadFeedbackModule(guild);

            if (config.AntiSpamConfig != null)
                LoadAntiSpamModule(guild);

            if (config.LoggingConfig != null)
                LoadLoggingModule(guild);

            if (config.ModnoteConfig != null)
                await LoadModnoteModule(guild);

            if (config.VoiceConfig != null)
                await LoadVoiceChannelModule(guild, config.VoiceConfig);

            await RegisterModules(guild);
        }
    }

    private async Task RegisterLanguages()
    {
        Log.Information("Registering Language configs for {ClientName}", _botModel.Name);

        var languageProvider = _serviceProvider.GetRequiredService<ILanguageProvider>();

        foreach (var config in _botModel.Configs!)
        {
            languageProvider.Register(new LanguageInfo(config.LinkedGuild.Snowflake, _client.CurrentUser.Id,
                LanguageProvider.GetCultureFromString(config.LinkedGuild.DefaultLanguage)));

            Log.Debug("Registered {ClientName} - {GuildName} - {Language}", _botModel.Name, config.LinkedGuild.Name,
                config.LinkedGuild.DefaultLanguage);
        }
    }

    private async Task CreateExtensions()
    {
        Log.Information("Creating Extensions");
        await _interactionService.AddModuleAsync<FeedbackExtension>(_serviceProvider);
        await _interactionService.AddModuleAsync<ModnoteExtension>(_serviceProvider);
        await _interactionService.AddModuleAsync<VoiceChannelCommandHandler>(_serviceProvider);

        _client.InteractionCreated += async interaction =>
        {
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
        };
    }
}