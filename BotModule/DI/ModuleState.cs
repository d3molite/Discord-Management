using BotModule.Extensions.VoiceChannels;
using Discord.WebSocket;

namespace BotModule.DI;

public class ModuleState : IModuleState
{
    public ModuleState()
    {
        FeedbackCommandState = new Dictionary<Guid, SocketGuildUser>();
        ModnoteCommandState = new Dictionary<Guid, SocketGuildUser>();
        VoiceChannelStates = new List<VoiceChannelState>();
    }

    public List<VoiceChannelState> VoiceChannelStates { get; set; }

    public Dictionary<Guid, SocketGuildUser> FeedbackCommandState { get; set; }

    public Dictionary<Guid, SocketGuildUser> ModnoteCommandState { get; set; }
}