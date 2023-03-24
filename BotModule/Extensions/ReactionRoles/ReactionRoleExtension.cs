using BotModule.DI;
using BotModule.Extensions.Base;
using BotModule.Extensions.Core;
using DB.Models.Configs.Extensions;
using DB.Repositories;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace BotModule.Extensions.ReactionRoles;

public class ReactionRoleExtension : ClientExtension
{
    public ReactionRoleExtension(
        DiscordSocketClient client,
        string botName,
        ILanguageProvider languageProvider) : base(client, botName, languageProvider)
    {
        Client.ReactionAdded += (message, channel, reaction) => ReactionHandler(message, channel, reaction, false);
        Client.ReactionRemoved += (message, channel, reaction) => ReactionHandler(message, channel, reaction, true);

        Log.Information("Reaction Roles attached to {ClientName}", BotName);
    }

    private static async Task ReactionHandler(
        Cacheable<IUserMessage, ulong> message,
        Cacheable<IMessageChannel, ulong> channel,
        SocketReaction reaction,
        bool remove)
    {
        if (!TryCreateReactionObject(message, channel, reaction, out var reactionObject)) return;

        if (!TryGetConfig(reactionObject, out var reactionRoleConfigs)) return;

        foreach (var config in reactionRoleConfigs)
        {
            if (reactionObject.MessageId != config.Message.Snowflake) continue;

            if (!TryGetRole(config, reactionObject, out var role)) continue;

            if (remove)
            {
                await reactionObject.User!.RemoveRoleAsync(role);
                Log.Debug("Removed {Role} from {User} in {Guild}", role!.Name, reactionObject.User!.DisplayName,
                    reactionObject.Guild!.Name);
                continue;
            }

            await reactionObject.User!.AddRoleAsync(role);
            Log.Debug("Added {Role} to {User} in {Guild}", role!.Name, reactionObject.User!.DisplayName,
                reactionObject.Guild!.Name);
        }
    }

    private static bool TryGetRole(
        ReactionRoleConfig config,
        ReactionObject reactionObject,
        out IRole? role)
    {
        var matchingConfig =
            config.ReactionRoleItems.FirstOrDefault(x => x.Emoji.EmojiString == reactionObject.EmojiName);

        role = null;

        if (matchingConfig is null) return false;

        role = reactionObject.Guild?.GetRole(matchingConfig.Role.Snowflake);

        return true;
    }

    private static bool TryCreateReactionObject(
        Cacheable<IUserMessage, ulong> message,
        Cacheable<IMessageChannel, ulong> channel,
        SocketReaction reaction,
        out ReactionObject reactionObject)
    {
        reactionObject = ReactionObject.Create(message, channel, reaction).Result;
        return reactionObject.IsValid();
    }

    private static bool TryGetConfig(ReactionObject reactionObject, out ReactionRoleConfig[] configs)
    {
        var configItems = ReactionRoleConfigRepository.Get(reactionObject.GuildId);

        if (configItems == null || !configItems.Any())
        {
            configs = Array.Empty<ReactionRoleConfig>();
            return false;
        }

        configs = configItems;
        return true;
    }
}