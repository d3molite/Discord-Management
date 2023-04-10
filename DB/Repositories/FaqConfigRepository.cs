using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public static class FaqConfigRepository
{
    public static FaqConfig? Get(ulong guildId)
    {
        using var db = ApiDbContext.Get();
        return db.GuildConfigs.FirstOrDefault(x => x.LinkedGuild.Snowflake == guildId)?.FaqConfig;
    }
}