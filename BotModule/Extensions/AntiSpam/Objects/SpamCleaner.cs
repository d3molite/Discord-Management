using System.Timers;
using BotModule.DI;
using BotModule.Extensions.Logging;
using DB.Models.Configs.Extensions;
using Discord;
using Discord.WebSocket;
using Strings.Manager;

namespace BotModule.Extensions.AntiSpam.Objects;

public class SpamCleaner
{
	private readonly ILoggingProvider _logger;
	private readonly System.Timers.Timer _timer;
	
	private readonly ILanguageProvider _languageProvider;

	public SocketGuildUser User { get; set; }

	public SocketGuild Guild { get; set; }

	public AntiSpamConfig Config { get; set; }

	public UserMessageCache Queue { get; set; }
	public event EventHandler SpamDeleted;

	public SpamCleaner(ILoggingProvider logger)
	{
		_logger = logger;
		
		_timer = new System.Timers.Timer(TimeSpan.FromSeconds(2).Milliseconds);
		_timer.Elapsed += DeleteSpam;
		
		_timer.Start();
	}

	public void Restart()
	{
		_timer.Stop();
		_timer.Start();
	}

	private async void DeleteSpam(object? sender, ElapsedEventArgs e)
	{
		throw new NotImplementedException();
	}
	
	private async Task Timeout(SocketGuildUser user)
	{
		await user.SetTimeOutAsync(TimeSpan.FromDays(3), new RequestOptions()
		{
			AuditLogReason = "Spam detected by {BotName}"
		});
	}

	private async Task Mute(SocketGuildUser user, SocketRole muted)
	{
		var roles = user.Roles.Where(x => x.Name != "everyone").ToArray();
		await user.RemoveRolesAsync(roles);
		await user.AddRoleAsync(muted);
	}
	
}