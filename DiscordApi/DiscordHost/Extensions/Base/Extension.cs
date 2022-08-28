using System.Globalization;
using System.Resources;
using Discord;
using DiscordApi.Data;
using DiscordApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions;

public class Extension
{
    public Extension(string botName)
    {
        BotName = botName;
    }

    protected string BotName { get; set; }

    public ResourceManager Resources { get; set; }

    protected string GetResource(string key, CultureInfo culture)
    {
        return Resources.GetString(key, culture)!.Replace("\\n", "\n");
    }

    protected string GetDiscriminatedUser(IUser user)
    {
        return user.Username + "#" + user.Discriminator.PadLeft(4, '0');
    }

    protected bool TryGetConfig<T>(ulong guildId, out T config)
        where T : new()
    {
        using var context = AppDBContext.Get();

        var include = typeof(BotConfig).GetProperties().Single(x => x.PropertyType == typeof(T));

        var guildName = context.Guilds.Single(x => x.GuildID == guildId).Name;

        // check if a config is available for this bot server constellation
        var conf = context.Configs
            .Where(p => p.RelatedBot.Name == BotName)
            .Where(q => q.RelatedGuild.GuildID == guildId)
            .Include(include.Name)
            .ToList();

        if (!conf.Any())
        {
            Log.Debug("No Config for {ClientName} in {GuildID}", BotName, guildName);
            config = new T();
            return false;
        }

        var c = conf.Single();
        var v = include.GetValue(c, null);

        if (v == null)
        {
            Log.Debug("No {ObjectType} for {ClientName} in {GuildID}", typeof(T), BotName, guildName);
            config = new T();
            return false;
        }

        config = (T)v;
        return true;
    }
}