using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApi.Models;

public class Message
{
    public int ID { get; set; }

    public ulong MessageID { get; set; }

    public string? Description { get; set; }

    [ForeignKey("MessageGuildID")] public virtual Guild RelatedGuild { get; set; }

    public override string ToString()
    {
        return Description != null ? Description : MessageID.ToString();
    }
}