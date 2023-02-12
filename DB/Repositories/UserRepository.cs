using DB.Models;

namespace DB.Repositories;

public class UserRepository
{
    public static User? Get(ulong userId)
    {
        using var db = ApiDbContext.Get();
        return db.Users.FirstOrDefault(x => x.Snowflake == userId);
    }

    public static User GetOrCreate(ulong userId)
    {
        using var db = ApiDbContext.Get();
        var user = db.Users.FirstOrDefault(x => x.Snowflake == userId);

        if (user != null) return user;

        var newUser = new User { Snowflake = userId };

        db.Users.Add(newUser);
        db.SaveChanges();

        return newUser;
    }
}