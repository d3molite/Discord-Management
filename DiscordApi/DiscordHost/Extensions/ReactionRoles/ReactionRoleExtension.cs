using Discord;
using Discord.WebSocket;
using DiscordApi.Data;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.ReactionRoles;

public class ReactionRoleExtension : ClientExtension
{
    public ReactionRoleExtension(string botName, DiscordSocketClient client)
        : base(botName, client)
    {
        ReactionSetup();
    }

    private void ReactionSetup()
    {
        Client.ReactionAdded += ReactionAddedHandler;
        Client.ReactionRemoved += ReactionRemovedHandler;

        Log.Information("Reaction Roles attached to {ClientName}", BotName);
    }

    private bool TryGetConfig(ulong guildId, string emoji, ulong messageId, out ReactionRoleConfig? config)
    {
        using var db = AppDBContext.Get();

        try
        {
            var roleConfig = db.Configs
                .Where(x => x.RelatedBot.Name == BotName)
                .Where(x => x.RelatedGuild.GuildID == guildId)
                .Include(x => x.RoleConfigs).ThenInclude(y => y.RelatedEmoji)
                .Include(x => x.RoleConfigs).ThenInclude(y => y.RelatedMessage)
                .Include(x => x.RoleConfigs).ThenInclude(y => y.RelatedGuild)
                .Include(x => x.RoleConfigs).ThenInclude(y => y.RelatedRole)
                .Select(x =>
                    x.RoleConfigs
                        .Where(y => y.RelatedEmoji.EmojiString == emoji)
                        .Where(y => y.RelatedGuild.GuildID == guildId)
                        .Single(y => y.RelatedMessage.MessageID == messageId))
                .Single();
            config = roleConfig;
            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Exception in TryGetConfig {ex}", ex);
            config = null;
            return false;
        }
    }

    private async Task ReactionRemovedHandler(Cacheable<IUserMessage, ulong> arg1,
        Cacheable<IMessageChannel, ulong> arg2,
        SocketReaction arg3)
    {
        var messageId = arg1.Id;
        var channel = arg2.GetOrDownloadAsync().Result as SocketGuildChannel;
        var emoji = arg3.Emote.Name;

        if (TryGetConfig(channel!.Guild.Id, emoji, messageId, out var config))
        {
            var role = channel.Guild.GetRole(config!.RelatedRole.RoleID);
            var user = channel.Guild.GetUser(arg3.UserId);
            await user.RemoveRoleAsync(role);

            Log.Debug("ReactionRemovedEvent removed {RoleName} from {UserName}", role.Name, user.Username);
        }
    }

    private async Task ReactionAddedHandler(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2,
        SocketReaction arg3)
    {
        var messageId = arg1.Id;
        var channel = arg2.GetOrDownloadAsync().Result as SocketGuildChannel;
        var emoji = arg3.Emote.Name;


        if (TryGetConfig(channel!.Guild.Id, emoji, messageId, out var config))
        {
            var role = channel.Guild.GetRole(config!.RelatedRole.RoleID);
            var user = channel.Guild.GetUser(arg3.UserId);
            await user.AddRoleAsync(role);

            Log.Debug("ReactionAddedEvent added {RoleName} to {UserName}", role.Name, user.Username);
        }
    }
}