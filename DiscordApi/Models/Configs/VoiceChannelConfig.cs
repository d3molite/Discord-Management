namespace DiscordApi.Models;

public class VoiceChannelConfig : IConfig<VoiceChannelConfig>
{
    public int Id { get; set; }

    public ulong? OnlyAllowedIn { get; set; }

    public ulong CategoryId { get; set; }

    public Guild VoiceGuild { get; set; }
}