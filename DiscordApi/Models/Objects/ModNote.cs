using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApi.Models;

public class Modnote
{
    public int ID { get; set; }

    public Guild Guild { get; set; }

    public User User { get; set; }

    [ForeignKey("AuthorId")] public User Author { get; set; }

    public DateOnly Logged { get; set; }

    public string Note { get; set; }
}