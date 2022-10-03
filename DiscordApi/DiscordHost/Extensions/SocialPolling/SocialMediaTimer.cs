using System.Net.Http.Headers;
using Discord;
using Discord.WebSocket;
using DiscordApi.Data;
using DiscordApi.DiscordHost.Extensions.SocialPolling.DataParsing;
using DiscordApi.Models;
using DiscordApi.REST;
using Newtonsoft.Json.Linq;

namespace DiscordApi.DiscordHost.Extensions.SocialPolling;

public class SocialMediaTimer
{
    private readonly DiscordSocketClient _client;
    private readonly HttpClient _httpClient = new();
    private readonly PeriodicTimer _timer;
    private SocialMediaConfig _config;

    public SocialMediaTimer(DiscordSocketClient client, SocialMediaConfig config)
    {
        _client = client;
        _config = config;

        _timer = new PeriodicTimer(TimeSpan.FromMinutes(60));

        Task.Run(async () => await Check());
    }

    private async Task Check()
    {
        while (await _timer.WaitForNextTickAsync(CancellationToken.None))
        {
            var uri = GetTarget(_config.Type, _config.UserId);
            var oauth = LocalKeyStorage.GetTwitter();

            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oauth);

            var response = await _httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            ISocialsData? parsed;

            switch (_config.Type)
            {
                case SocialType.Twitter:
                    if (TwitterParser.TryParse(responseText, _config.LastPosted.ToString(), out parsed))
                    {
                        await SendEmbed(parsed!);
                    }

                    break;
                case SocialType.Facebook:
                    break;
                case SocialType.Instagram:
                    break;
            }
        }
    }

    private async Task SendEmbed(ISocialsData parsed)
    {
        if (!string.IsNullOrEmpty(parsed.Text))
        {
            var channel = await _client.GetChannelAsync(_config.ChannelId);
            if (channel is IMessageChannel messageChannel)
            {
                await messageChannel.SendMessageAsync("", embed: GenerateSocialsEmbed(parsed));
            }
        }
    }

    private Embed GenerateSocialsEmbed(ISocialsData socialsData)
    {
        var type = "";

        switch (socialsData.Type)
        {
            case SocialType.Twitter:
                type = "Tweet";
                break;
            case SocialType.Facebook:
                break;
            case SocialType.Instagram:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var builder = new EmbedBuilder();
        builder.Title = $"Neuer {type}";
        builder.AddField($"{type} von {_config.UserHandle}", socialsData.Text);
        builder.Url = socialsData.Url;
        builder.ThumbnailUrl = socialsData.MediaUrl;
        return builder.Build();
    }

    private async Task<string> ParseTwitter(string response)
    {
        var items = JObject.Parse(response);

        var latestTweet = items["data"][0];

        if (ulong.Parse(latestTweet["id"].Value<string>()) != _config.LastPosted)
        {
            using var dbContext = AppDBContext.Get();

            var config = dbContext.SocialConfigs.First(x => x.Id == _config.Id);
            config.LastPosted = ulong.Parse(latestTweet["id"].Value<string>());

            dbContext.SaveChanges();

            _config = config;

            return latestTweet["text"].Value<string>();
        }

        return "";
    }

    private Uri GetTarget(SocialType type, string id)
    {
        return type switch
        {
            SocialType.Twitter => new Uri(
                $"https://api.twitter.com/2/users/{id}/tweets?max_results=5&expansions=attachments.media_keys&media.fields=preview_image_url,url"),
            SocialType.Facebook => new Uri(""),
            SocialType.Instagram => new Uri(""),
            _ => new Uri("")
        };
    }
}