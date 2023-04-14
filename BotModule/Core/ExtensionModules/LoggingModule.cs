using BotModule.DI;
using BotModule.Extensions.Logging;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private bool _alreadyRegistered;

    private void LoadLoggingModule(IGuild guild)
    {
        LogStartupAction($"Loaded Logging Module for {Name} in {guild.Name}");
        var loggingProvider = _serviceProvider.GetRequiredService<ILoggingProvider>();
        var languageProvider = _serviceProvider.GetRequiredService<ILanguageProvider>();
        var loggingExtension = new LoggingExtension(_client, Name, languageProvider, _alreadyRegistered);

        var info = new LoggingInfo(guild, loggingExtension);

        loggingProvider.Register(info, _client);

        _alreadyRegistered = true;
    }
}