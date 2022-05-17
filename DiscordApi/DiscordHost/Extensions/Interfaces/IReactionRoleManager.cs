using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Extensions.Base;

public class ReactionRoleManager : ClientExtension
{
    public ReactionRoleManager(string botName, DiscordSocketClient client)
        : base(botName, client)
    {
    }
}