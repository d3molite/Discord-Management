namespace DiscordApi.Models;

public class ReactionRoleConfig
{
    public int ID { get; set; }

    public Guild RelatedGuild { get; set; }

    public Message RelatedMessage { get; set; }

    public Emoji RelatedEmoji { get; set; }

    public Role RelatedRole { get; set; }
}