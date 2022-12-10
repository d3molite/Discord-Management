namespace DB.Models.Configs.Extensions;

public class LoggingConfig
{
	public int Id { get; set; }
	public GuildChannel LoggingChannel { get; set; } = null!;

	public bool LogMessageDeleted { get; set; }

	public bool LogUserJoined { get; set; }

	public bool LogUserLeft { get; set; }

	public bool LogUserBanned { get; set; }
}