using DB.Models.Base;

namespace DB.Models.Configs.Extensions;

#nullable disable

public class ReactionRoleConfig
{
	public int Id { get; set; }
	
	public Message Message { get; set; }
	
	// ReSharper disable once CollectionNeverUpdated.Global
	public List<ReactionRoleItem> ReactionRoleItems { get; set; }
}