using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public static class VoiceConfigRepository
{
    public static VoiceConfig? Get(ulong guildId)
    {
        using var db = ApiDbContext.Get();
        return db.GuildConfigs.FirstOrDefault(x => x.LinkedGuild.Snowflake == guildId)?.VoiceConfig;
    }
}