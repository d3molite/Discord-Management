using BotModule.DI;
using BotModule.Extensions.AntiSpam;
using BotModule.Extensions.Logging;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private AntiSpamExtension _antiSpamExtension;

    private void LoadAntiSpamModule(IGuild guild)
    {
        Log.Information("Loaded AntiSpam Module for {BotName} in {GuildName}",
            Name,
            guild.Name);

        _antiSpamExtension = new AntiSpamExtension(
            _client,
            Name,
            _serviceProvider.GetRequiredService<ILoggingProvider>(),
            _serviceProvider.GetRequiredService<ILanguageProvider>());
    }
}