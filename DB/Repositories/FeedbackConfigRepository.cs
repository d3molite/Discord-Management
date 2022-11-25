using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public static class FeedbackConfigRepository
{
    public static bool TryGet(ulong guildId, out FeedbackConfig? config)
    {
        using var db = ApiDbContext.Get();

        config = db.FeedbackConfigs.FirstOrDefault(x => x.TargetChannel.LinkedGuild.Snowflake == guildId);

        return config != null;
    }
}