using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Utils;

public class VoiceChannelState
{
    public ulong BotId { get; set; }

    public SocketVoiceChannel VoiceChannel { get; set; }

    public bool UsersPresentInLastCheck { get; set; }
}