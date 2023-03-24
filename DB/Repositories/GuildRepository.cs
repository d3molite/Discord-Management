using DB.Models;

namespace DB.Repositories;

public class GuildRepository
{
    public static Guild? Get(ulong guildId)
    {
        using var db = ApiDbContext.Get();
        return db.Guilds.FirstOrDefault(x => x.Snowflake == guildId);
    }

    public static Guild GetOrCreate(ulong guildId)
    {
        using var db = ApiDbContext.Get();

        var guild = db.Guilds.FirstOrDefault(x => x.Snowflake == guildId);

        if (guild != null) return guild;

        return new Guild { Snowflake = guildId };
    }
}