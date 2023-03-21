using DB.Models.Base;

namespace DB.Models.Configs.Extensions;

public class MessageReactionItem
{
    public int Id { get; set; }

    public Emoji? ReactionEmoji { get; set; }

    public string? ReactionMessage { get; set; }

    public bool EmojiOnly { get; set; }

    public string ReactionTrigger { get; set; }

    public int ReactionChance { get; set; }
}