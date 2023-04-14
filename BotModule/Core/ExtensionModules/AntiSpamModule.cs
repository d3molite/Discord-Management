using BotModule.DI;
using BotModule.Extensions.AntiSpam;
using BotModule.Extensions.Logging;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private AntiSpamExtension _antiSpamExtension;

    private void LoadAntiSpamModule(IGuild guild)
    {
        LogStartupAction($"Loaded AntiSpam Module for {Name} in {guild.Name}");

        _antiSpamExtension = new AntiSpamExtension(
            _client,
            Name,
            _serviceProvider.GetRequiredService<ILoggingProvider>(),
            _serviceProvider.GetRequiredService<ILanguageProvider>());
    }
}