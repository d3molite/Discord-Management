using DB.Models;

namespace DB.Repositories;

public class BotRepository
{
    public static IEnumerable<Bot> Get()
    {
        using var db = DiscordDbContext.Get();
        return db.Bots.ToList();
    }

    public static Bot? Get(int id)
    {
        using var db = DiscordDbContext.Get();
        return db.Bots.FirstOrDefault(x => x.Id == id);
    }
}