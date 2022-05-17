using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Utils;

public class StateHandler : IStateHandler
{
    public StateHandler()
    {
        ModalCommandState = new Dictionary<Guid, SocketGuildUser>();
        ESportsCommandState = new Dictionary<Guid, ReadyCheckState>();
    }

    public Dictionary<Guid, SocketGuildUser> ModalCommandState { get; set; }
    public Dictionary<Guid, ReadyCheckState> ESportsCommandState { get; set; }
}