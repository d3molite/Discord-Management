using BotModule.DI;
using BotModule.Extensions.Logging;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private void LoadLoggingModule(IGuild guild)
    {
        Log.Debug("Loaded Logging Module for {BotName} in {GuildName}", Name, guild.Name);
        var loggingProvider = _serviceProvider.GetRequiredService<ILoggingProvider>();
        var languageProvider = _serviceProvider.GetRequiredService<ILanguageProvider>();
        var loggingExtension = new LoggingExtension(_client, Name, languageProvider);

        var info = new LoggingInfo(guild, loggingExtension);

        loggingProvider.Register(info);
    }
}