using DiscordApi.DiscordHost.Bot;

namespace DiscordApi.Services.Interfaces;

public interface IBotHostService
{
    List<DiscordBot> BotWrappers { get; set; }
}