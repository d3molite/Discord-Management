using DB.Models.Configs.Extensions;
using Discord.WebSocket;

namespace BotModule.Extensions.AntiSpam.Objects;

public class SpamCleaner
{
	public SocketGuildUser User { get; set; }

	public SocketGuild Guild { get; set; }

	public AntiSpamConfig Config { get; set; }

	public UserMessageCache Queue { get; set; }

	public event EventHandler SpamDeleted;
}