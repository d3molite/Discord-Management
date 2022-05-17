using DiscordApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DiscordApi.Data.Repositories;

public static class ModNoteRepository
{
    public static IEnumerable<Modnote> Get(ulong guildId, ulong userid)
    {
        using var db = AppDBContext.Get();
        return db.Modnotes
            .Where(x => x.User.UserID == userid)
            .Where(x => x.Guild.GuildID == guildId)
            .Include(x => x.Author).ToList();
    }

    public static Modnote? Get(ulong guildId, ulong userid, int position)
    {
        var notes = Get(guildId, userid);
        if (notes.Count() > position - 1) return notes.ElementAt(position - 1);

        return null;
    }

    public static bool Create(ulong guildid, ulong userid, ulong authorid, string modnote)
    {
        using var db = AppDBContext.Get();

        var user = UserRepository.Get(userid, db);
        var guild = GuildRepository.Get(guildid, db);
        var author = UserRepository.Get(authorid, db);

        if (user == null) user = new User { UserID = userid };
        if (author == null) author = new User { UserID = userid };
        if (guild == null) guild = new Guild { GuildID = guildid };

        db.Modnotes.Add(new Modnote
        {
            User = user,
            Guild = guild,
            Note = modnote,
            Author = author,
            Logged = DateOnly.FromDateTime(DateTime.Now)
        });

        try
        {
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Exception occurred in creating modnote. {exception}", ex);
            return false;
        }
    }

    public static bool Delete(ulong guildid, ulong userid, int position)
    {
        using var db = AppDBContext.Get();
        var note = Get(guildid, userid, position);
        if (note != null)
        {
            db.Modnotes.Remove(note);
            db.SaveChanges();
            return true;
        }

        return false;
    }
}