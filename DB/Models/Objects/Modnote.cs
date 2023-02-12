using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models.Objects;

public class Modnote
{
    public int Id { get; set; }

    public Guild Guild { get; set; } = null!;

    public User User { get; set; } = null!;

    [ForeignKey("AuthorId")] public User Author { get; set; } = null!;

    public DateOnly DateLogged { get; set; }

    public string Note { get; set; } = null!;
}