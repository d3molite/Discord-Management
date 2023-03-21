namespace DB.Models.Configs.Extensions;

public class VoiceConfig
{
    public int Id { get; set; }

    public GuildChannel? RestrictedChannel { get; set; }

    public GuildChannel Category { get; set; } = null!;
}