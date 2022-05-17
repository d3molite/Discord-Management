using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Extensions.Base;

public class ClientExtension : Extension
{
    public ClientExtension(string botName, DiscordSocketClient client)
        : base(botName)
    {
        Client = client;
    }

    public DiscordSocketClient Client { get; set; }
}