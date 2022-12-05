using Discord.WebSocket;

namespace BotModule.Extensions.Base;

public class ClientExtension : Extension
{
	public ClientExtension(DiscordSocketClient client, string botName) : base(botName)
	{
		Client = client;
	}

	public DiscordSocketClient Client { get; }
}