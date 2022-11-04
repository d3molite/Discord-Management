using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Utils;

public interface IStateHandler
{
    public Dictionary<Guid, SocketGuildUser> ModalCommandState { get; set; }

    public Dictionary<Guid, ReadyCheckState> ESportsCommandState { get; set; }

    public List<VoiceChannelState> VoiceChannelStates { get; set; }
}