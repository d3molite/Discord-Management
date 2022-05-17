using DiscordApi.DiscordHost.Bot;

namespace DiscordApi.Services;

public interface IBotHostService
{
    List<DiscordBot> BotWrappers { get; set; }
}