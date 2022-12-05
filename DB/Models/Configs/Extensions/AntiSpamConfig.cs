namespace DB.Models.Configs.Extensions;

public class AntiSpamConfig
{
	public int Id { get; set; }

	public string? IgnorePrefixes { get; set; }

	public GuildRole? MutedRole { get; set; }
}