using DB.Models.Interfaces;
using Enum.DatabaseEnums;

namespace DB.Models;

public class GuildChannel : DiscordItem
{
    public GuildChannelType ChannelType { get; set; }

    public Guild LinkedGuild { get; set; } = null!;
}