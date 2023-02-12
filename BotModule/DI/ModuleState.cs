using Discord.WebSocket;

namespace BotModule.DI;

public class ModuleState : IModuleState
{
    public ModuleState()
    {
        FeedbackCommandState = new Dictionary<Guid, SocketGuildUser>();
    }

    public Dictionary<Guid, SocketGuildUser> FeedbackCommandState { get; set; }

    public Dictionary<Guid, SocketGuildUser> ModnoteCommandState { get; set; }
}