using DiscordApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscordApi.Services;

public static class LocalizationService
{
    static LocalizationService()
    {
        LocaleInfo = new Dictionary<string, string>();
        Initialize();
    }

    public static Dictionary<string, string> LocaleInfo { get; set; }

    public static void Initialize()
    {
        using var db = AppDBContext.Get();

        var configs = db.Configs
            .Include(x => x.RelatedGuild)
            .Include(x => x.RelatedBot).ToList();

        LocaleInfo =
            configs.ToDictionary(key => key.RelatedBot.Name + "_" + key.RelatedGuild.GuildID, value => value.Locale);
    }

    public static string GetLocale(string name, ulong guildId)
    {
        var key = string.Format("{0}_{1}", name, guildId);

        if (LocaleInfo.TryGetValue(key, out var locale))
        {
            return locale.ToLower() switch
            {
                "de" => "de-de",
                _ => "Default Culture"
            };
        }

        return "en";
    }
}