using BotModule.Extensions.Logging;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace BotModule.DI;

public class LoggingProvider : ILoggingProvider
{
    private readonly List<LoggingInfo> _loggers = new();

    public void Register(LoggingInfo loggingCombo)
    {
        if (_loggers.Contains(loggingCombo)) return;

        _loggers.Add(loggingCombo);
        Log.Debug("Registered Logger for {Client}", loggingCombo.Logger.BotName);
    }

    public ILoggingExtension? Retrieve(DiscordSocketClient client, IGuild guild)
    {
        return _loggers.FirstOrDefault(x => x.Logger.Client == client && x.Guild == guild)?.Logger;
    }

    public ILoggingExtension? Retrieve(ulong clientId, ulong guildId)
    {
        return _loggers.FirstOrDefault(x => x.Logger.Client.CurrentUser.Id == clientId)?.Logger;
    }
}