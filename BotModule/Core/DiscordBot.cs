using BotModule.DI;
using DB.Models;
using DB.Models.Configs;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Serilog;

namespace BotModule.Core;

public sealed class DiscordBot
{
    private readonly Bot _botModel;
    private readonly DiscordSocketClient _client;

    private readonly DiscordSocketConfig _config = new()
        { MessageCacheSize = 100, GatewayIntents = GatewayIntents.All, UseInteractionSnowflakeDate = false };

    private readonly IConfigProvider _configProvider;
    private readonly IEnumerable<GuildConfig> _configs;
    private readonly InteractionService _interactionService;

    public DiscordBot(DiscordSocketClient client, Bot botModel)
    {
        _client = client;
        _botModel = botModel;

        _client = new DiscordSocketClient(_config);
        _interactionService = new InteractionService(_client);
    }

    public async Task StartBot()
    {
        await _client.LoginAsync(TokenType.Bot, "");
    }

    private async Task LoadExtensions()
    {
        Log.Information("Loading Extensions for {ClientName}", _botModel.Name);
    }
}