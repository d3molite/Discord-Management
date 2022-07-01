namespace DiscordApi.Models;

public class MessageReaction
{
    public int ID { get; set; }

    public Emoji? ReactionEmoji { get; set; }

    public string? ReactionMessage { get; set; }

    public bool EmojiOnly { get; set; }

    public string ReactionTrigger { get; set; }

    public int ReactionChance { get; set; }
}