namespace DiscordApi.Models;

public enum SocialType
{
    Twitter,
    Facebook,
    Instagram
}

public class SocialMediaConfig
{
    public int Id { get; set; }

    public SocialType Type { get; set; }

    public string UserId { get; set; }

    public string UserHandle { get; set; }

    public ulong LastPosted { get; set; }

    public ulong ChannelId { get; set; }
}