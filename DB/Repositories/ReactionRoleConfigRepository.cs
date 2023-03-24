using DB.Models.Configs.Extensions;

namespace DB.Repositories;

public class ReactionRoleConfigRepository
{
	public static ReactionRoleConfig[]? Get(ulong guildId)
	{
		using var db = ApiDbContext.Get();

		return db.GuildConfigs.FirstOrDefault(x => x.LinkedGuild.Snowflake == guildId)?.ReactionRoleConfigs?.ToArray();
	}
}