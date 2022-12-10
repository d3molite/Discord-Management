using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public static class LoggingConfigRepository
{
	public static LoggingConfig? Get(ulong guildId)
	{
		using var db = ApiDbContext.Get();
		return db.GuildConfigs.First(x => x.LinkedGuild.Snowflake == guildId).LoggingConfig;
	}
}