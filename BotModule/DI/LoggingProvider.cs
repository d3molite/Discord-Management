using BotModule.Extensions.Logging;
using Discord.WebSocket;

namespace BotModule.DI;

public class LoggingProvider : ILoggingProvider
{
	private readonly List<LoggingExtension> _loggers = new();

	public void Register(LoggingExtension loggingExtension)
	{
		if (_loggers.Contains(loggingExtension)) return;
		_loggers.Add(loggingExtension);
	}

	public LoggingExtension? Retrieve(DiscordSocketClient client) => throw new NotImplementedException();
}