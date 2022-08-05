namespace DiscordApi.Models;

public class FeedbackConfig : IConfig<FeedbackConfig>
{
    public int ID { get; set; }

    public bool AddReactions { get; set; }

    public ulong FeedbackChannelID { get; set; }
}