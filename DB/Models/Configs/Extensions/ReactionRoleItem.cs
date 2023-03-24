#nullable disable

using DB.Models.Base;

namespace DB.Models.Configs.Extensions;

public class ReactionRoleItem
{
	public int Id { get; set; }

	public Emoji Emoji { get; set; }
	
	public GuildRole Role { get; set; }
}