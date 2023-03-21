using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public static class MessageReactionConfigRepository
{
    public static MessageReactionConfig? Get(ulong guildId)
    {
        using var db = ApiDbContext.Get();
        return db.GuildConfigs.FirstOrDefault(x => x.LinkedGuild.Snowflake == guildId)?.MessageReactionConfig;
    }
}