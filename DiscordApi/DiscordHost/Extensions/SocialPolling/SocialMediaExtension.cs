using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.Models;

namespace DiscordApi.DiscordHost.Extensions.SocialPolling;

public class SocialMediaExtension : ClientExtension
{
    private readonly List<SocialMediaTimer> _timers = new();

    public SocialMediaExtension(string botName, DiscordSocketClient client, IEnumerable<BotConfig> botConfigs)
        : base(botName, client)
    {
        foreach (var config in botConfigs)
        {
            foreach (var socialMedia in config.SocialMediaConfigs)
                _timers.Add(new SocialMediaTimer(client, socialMedia));
        }
    }
}