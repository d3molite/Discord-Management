using BotModule.DI;
using BotModule.Extensions.ReactionRoles;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private ReactionRoleExtension _reactionRoleExtension;

    private void LoadReactionRoleExtension(IGuild guild)
    {
        LogStartupAction($"Loaded Reaction Role Module for {Name} in {guild.Name}");

        _reactionRoleExtension = new ReactionRoleExtension(
            _client,
            Name,
            _serviceProvider.GetRequiredService<ILanguageProvider>());
    }
}