using BotModule.DI;
using BotModule.Extensions.MessageReaction;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private MessageReactionExtension _messageReactionExtension;

    private void LoadMessageReactionExtension(IGuild guild)
    {
        LogStartupAction($"Loaded Message Reaction Module for {Name} in {guild.Name}");

        _messageReactionExtension = new MessageReactionExtension(
            _client,
            Name,
            _serviceProvider.GetRequiredService<ILanguageProvider>());
    }
}