using Discord;
using Discord.WebSocket;

namespace BotModule.Extensions.Logging;

public interface ILoggingProvider
{
    public void Register(LoggingInfo loggingCombo);

    public ILoggingExtension? Retrieve(DiscordSocketClient client, IGuild guild);

    public ILoggingExtension? Retrieve(ulong clientId, ulong guildId);
}