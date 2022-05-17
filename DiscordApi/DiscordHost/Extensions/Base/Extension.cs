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

    protected bool TryGetConfig<T>(ulong guildId, out T config)
        where T : new()
    {
        using var context = AppDBContext.Get();

        var include = typeof(BotConfig).GetProperties().Single(x => x.PropertyType == typeof(T));

        // check if a config is available for this bot server constellation
        var conf = context.Configs
            .Where(p => p.RelatedBot.Name == BotName)
            .Where(q => q.RelatedGuild.GuildID == guildId)
            .Include(include.Name)
            .ToList();

        if (!conf.Any())
        {
            Log.Debug("No Config for {ClientName} in {GuildID}", BotName, guildId);
            config = new T();
            return false;
        }

        var c = conf.Single();
        var v = include.GetValue(c, null);

        if (v == null)
        {
            Log.Debug("No {ObjectType} for {ClientName} in {GuildID}", nameof(T), BotName, guildId);
            config = new T();
            return false;
        }

        config = (T)v;
        return true;
    }
}