using System.Text.RegularExpressions;
using DiscordApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordApi.DiscordHost.Extensions.SocialPolling.DataParsing;

public static class TwitterParser
{
    public static bool TryParse(string response, string id, out ISocialsData? parsed)
    {
        var obj = JObject.Parse(response);
        var data = obj["data"].ToString();
        var items = JsonConvert.DeserializeObject<List<TwitterData>>(data);

        if (items == null || !items.Any())
        {
            parsed = null;
            return false;
        }

        parsed = items[0];
        parsed.Type = SocialType.Twitter;
        parsed.SetUrl();

        try
        {
            var mediaKey = obj["data"][0]["attachments"]["media_keys"][0].ToString();

            var media = JsonConvert.DeserializeObject<List<TwitterMedia>>(obj["includes"]["media"].ToString());
            var url = media.FirstOrDefault(x => x.MediaKey == mediaKey).Url;

            parsed.MediaUrl = url;
        }
        catch
        {
            // ignored
        }


        return items[0].Id != id;
    }
}

public sealed class TwitterData : ISocialsData
{
    [JsonProperty("edit_history_tweet_ids")]
    public List<string> EditHistory { get; set; }

    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("text")] public string Text { get; set; }

    [JsonIgnore] public SocialType Type { get; set; }

    [JsonIgnore] public string Url { get; set; }

    [JsonIgnore] public string MediaUrl { get; set; }

    public void SetUrl()
    {
        var regex = new Regex(@"https://t.co/[\w]+");
        var match = regex.Match(Text);

        if (match.Success)
        {
            Url = match.Value;
        }
    }
}

internal sealed class TwitterMedia
{
    [JsonProperty("media_key")] public string MediaKey { get; set; }

    [JsonProperty("url")] public string Url { get; set; }
}