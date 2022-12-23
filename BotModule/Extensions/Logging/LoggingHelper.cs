using Discord;

namespace BotModule.Extensions.Logging;

public static class LoggingHelper
{
    public static string GetDiscriminatedUser(this IUser user)
    {
        return user.Username + "#" + user.Discriminator.PadLeft(4, '0');
    }
}