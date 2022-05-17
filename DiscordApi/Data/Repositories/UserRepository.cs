using DiscordApi.Models;

namespace DiscordApi.Data.Repositories;

public static class UserRepository
{
    public static User? Get(ulong userid, AppDBContext? db = null)
    {
        db ??= AppDBContext.Get();
        var user = db.Users.Where(x => x.UserID == userid);
        return user.Any() ? user.First() : null;
    }
}