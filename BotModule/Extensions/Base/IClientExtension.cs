using Discord.WebSocket;

namespace BotModule.Extensions.Base;

public interface IClientExtension
{
    public DiscordSocketClient Client { get; }
}