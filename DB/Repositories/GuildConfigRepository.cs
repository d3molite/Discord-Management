using DB.Models.Configs;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class GuildConfigRepository
{
    public static IEnumerable<GuildConfig> Get()
    {
        var ret = new List<GuildConfig>();

        using var db = ApiDbContext.Get();

        var guildConfigs =
            db.GuildConfigs?
                .Include(x => x.AntiSpamConfig)
                .Include(x => x.FeedbackConfig)
                .ToList();

        if (guildConfigs != null)
        {
            ret.AddRange(guildConfigs);
        }

        return ret;
    }
}