using DiscordApi.Models;

namespace DiscordApi.Data.Repositories;

public static class GuildRepository
{
    public static Guild? Get(ulong guildId, AppDBContext? db = null)
    {
        db ??= AppDBContext.Get();
        var guild = db.Guilds.Where(x => x.GuildID == guildId);
        return guild.Any() ? guild.First() : null;
    }
}