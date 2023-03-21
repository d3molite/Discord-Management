namespace DB.Models.Configs.Extensions;

public class MessageReactionConfig
{
    public int Id { get; set; }

    public List<MessageReactionItem> MessageReactions { get; set; } = null!;
}