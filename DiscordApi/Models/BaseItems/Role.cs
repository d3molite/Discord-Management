using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordApi.Models;

public class Role
{
    public int ID { get; set; }
    public string Name { get; set; }
    public ulong RoleID { get; set; }

    [ForeignKey("RoleGuildID")] public virtual Guild RelatedGuild { get; set; }

    public override string ToString()
    {
        return Name;
    }
}