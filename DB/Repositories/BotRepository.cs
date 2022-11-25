using DB.Models;

namespace DB.Repositories;

public static class BotRepository
{
	public static IEnumerable<Bot> Get()
	{
		var ret = new List<Bot>();
		using var db = ApiDbContext.Get();
		var bots = db.Bots?.ToList();

		if (bots != null)
			ret.AddRange(bots);

		return ret;
	}

	public static Bot? Get(int id)
	{
		using var db = ApiDbContext.Get();
		return db.Bots?.FirstOrDefault(x => x.Id == id);
	}

	public static Bot UpdateSnowFlake(int id, ulong snowflake)
	{
		using var db = ApiDbContext.Get();
		var bot = db.Bots.First(x => x.Id == id);
		bot.Snowflake = snowflake;
		db.SaveChanges();
		return bot;
	}

	public static Bot? Get(ulong snowflake)
	{
		using var db = ApiDbContext.Get();
		return db.Bots?.FirstOrDefault(x => x.Snowflake == snowflake);
	}
}