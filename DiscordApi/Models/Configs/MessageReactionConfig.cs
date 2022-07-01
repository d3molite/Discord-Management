namespace DiscordApi.Models;

public class MessageReactionConfig : IConfig<MessageReactionConfig>
{
    public int ID { get; set; }
    public List<MessageReaction> MessageReactions { get; set; }
}