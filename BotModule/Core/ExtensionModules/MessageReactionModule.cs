using BotModule.DI;
using BotModule.Extensions.MessageReaction;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private MessageReactionExtension _messageReactionExtension;

    private void LoadMessageReactionExtension(IGuild guild)
    {
        Log.Information("Loaded Message Reaction Module for {BotName} in {GuildName}",
            Name,
            guild.Name);

        _messageReactionExtension = new MessageReactionExtension(
            _client,
            Name,
            _serviceProvider.GetRequiredService<ILanguageProvider>());
    }
}