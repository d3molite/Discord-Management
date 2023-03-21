using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public static class AntiSpamConfigRepository
{
    public static AntiSpamConfig? Get(ulong guildId)
    {
        using var db = ApiDbContext.Get();
        return db.GuildConfigs.FirstOrDefault(x => x.LinkedGuild.Snowflake == guildId)?.AntiSpamConfig;
    }

    public static AntiSpamConfig? GetFromContext(ulong guildId, ApiDbContext context)
    {
        return context.GuildConfigs.FirstOrDefault(x => x.LinkedGuild.Snowflake == guildId)?.AntiSpamConfig;
    }
}