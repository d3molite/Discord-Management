using DiscordApi.Models;

namespace DiscordApi.DiscordHost.Extensions.SocialPolling.DataParsing;

public interface ISocialsData
{
    public string Text { get; set; }

    public SocialType Type { get; set; }

    public string Url { get; set; }

    public string MediaUrl { get; set; }

    public string Id { get; set; }

    public void SetUrl();
}