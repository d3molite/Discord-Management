using DB.Models.Configs;

namespace DB.Models;

public class Bot
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public string? Token { get; set; }

    public string? Presence { get; set; }
    public List<GuildConfig>? Configs { get; set; }

    public bool IsActiveInDebug { get; set; }

    public bool IsActiveInRelease { get; set; }

    public override string ToString()
    {
        return Name ?? string.Empty;
    }
}