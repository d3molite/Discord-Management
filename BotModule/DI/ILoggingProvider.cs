using Discord.WebSocket;

namespace BotModule.Extensions.Logging;

public interface ILoggingProvider
{
	public void Register(LoggingExtension loggingExtension);

	public LoggingExtension? Retrieve(DiscordSocketClient client);
}