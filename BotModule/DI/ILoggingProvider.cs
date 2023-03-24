using Discord;
using Discord.WebSocket;

namespace BotModule.Extensions.Logging;

public interface ILoggingProvider
{
    public void Register(LoggingInfo loggingCombo, DiscordSocketClient client);

    public ILoggingExtension? Retrieve(DiscordSocketClient client, IGuild guild);

    public ILoggingExtension? Retrieve(ulong clientId, ulong guildId);
}