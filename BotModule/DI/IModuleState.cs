using Discord.WebSocket;

namespace BotModule.DI;

public interface IModuleState
{
    public Dictionary<Guid, SocketGuildUser> FeedbackCommandState { get; set; }
}