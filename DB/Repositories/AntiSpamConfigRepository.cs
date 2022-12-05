using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public class AntiSpamConfigRepository
{
	public static AntiSpamConfig? Get(ulong guildId)
	{
		using var db = ApiDbContext.Get();
		return db.GuildConfigs.FirstOrDefault(x => x.LinkedGuild.Snowflake == guildId)?.AntiSpamConfig;
	}
}