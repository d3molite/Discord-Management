using DiscordApi.Data;

namespace DiscordApi.Services;

public static class LocalizationService
{
    static LocalizationService()
    {
        LocaleInfo = new();
        Initialize();
    }
    
    public static Dictionary<string, string> LocaleInfo { get; set; }
    
    public static void Initialize()
    {
        
        using var db = AppDBContext.Get();

        foreach (var config in db.Configs)
        {
            LocaleInfo.Add(config.RelatedBot.Name, config.Locale);
        }
        
    }

    public static string GetLocale(string name)
    {
        if (!LocaleInfo.TryGetValue(name, out var locale)) return "en";

        return locale.ToLower() switch
        {
            "de" => "de-de",
            _ => "Default Culture"
        };
    }
}