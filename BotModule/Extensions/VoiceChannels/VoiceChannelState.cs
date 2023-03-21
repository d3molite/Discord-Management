using Discord.WebSocket;

namespace BotModule.Extensions.VoiceChannels;

public class VoiceChannelState
{
    public ulong BotId { get; set; }

    public bool UsersPresent { get; set; }

    public SocketVoiceChannel Channel { get; set; }
}