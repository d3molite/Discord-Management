using Newtonsoft.Json;

namespace DiscordApi.REST;

public static class LocalKeyStorage
{
    private static readonly string authPath = @"./REST/Auth.json";

    public static string GetTwitter()
    {
        using var r = new StreamReader(authPath);

        var json = r.ReadToEnd();
        var items = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        return items!["twitter_bearer_token"];
    }
}