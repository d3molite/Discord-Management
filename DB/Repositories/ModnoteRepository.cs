using DB.Models.Objects;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DB.Repositories;

public class ModnoteRepository
{
    public static IEnumerable<Modnote> Get(ulong guildId, ulong userid)
    {
        using var db = ApiDbContext.Get();
        return db.Modnotes
            .Where(x => x.User.Snowflake == userid)
            .Where(x => x.Guild.Snowflake == guildId)
            .Include(x => x.Author).ToList();
    }

    public static Modnote? Get(ulong guildId, ulong userid, int position)
    {
        var notes = Get(guildId, userid).ToArray();
        return notes.Length > position - 1 ? notes.ElementAt(position - 1) : null;
    }

    public static bool Create(ulong guildId, ulong userId, ulong authorId, string note)
    {
        using var db = ApiDbContext.Get();

        var user = UserRepository.GetOrCreate(userId);
        var author = UserRepository.GetOrCreate(authorId);
        var guild = GuildRepository.GetOrCreate(guildId);

        db.Modnotes.Add(new Modnote
        {
            User = user,
            Author = author,
            Guild = guild,
            Note = note,
            DateLogged = DateOnly.FromDateTime(DateTime.Now)
        });

        try
        {
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Exception occurred in creating Modnote. {exception}", ex);
            return false;
        }
    }

    public static bool Delete(ulong guildid, ulong userid, int position)
    {
        using var db = ApiDbContext.Get();
        var note = Get(guildid, userid, position);

        if (note == null) return false;

        db.Modnotes.Remove(note);
        db.SaveChanges();
        return true;
    }
}