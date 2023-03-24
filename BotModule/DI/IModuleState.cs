using BotModule.Extensions.VoiceChannels;
using Discord.WebSocket;

namespace BotModule.DI;

public interface IModuleState
{
    public Dictionary<Guid, SocketGuildUser> FeedbackCommandState { get; set; }

    public Dictionary<Guid, SocketGuildUser> ModnoteCommandState { get; set; }

    public List<VoiceChannelState> VoiceChannelStates { get; set; }
}