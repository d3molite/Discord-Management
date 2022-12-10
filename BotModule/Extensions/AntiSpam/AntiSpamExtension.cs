using BotModule.Extensions.AntiSpam.Objects;
using BotModule.Extensions.Base;
using BotModule.Extensions.Logging;
using DB.Repositories;
using Discord.WebSocket;
using Serilog;

namespace BotModule.Extensions.AntiSpam;

public class AntiSpamExtension : ClientExtension
{
	private const int QueueLength = 10;
	private readonly GuildMessageCache _messages = new(QueueLength);
	private readonly List<SpamCleaner> _spamCleaners = new();

	public AntiSpamExtension(DiscordSocketClient client, string botName) : base(client, botName)
	{
		Client.MessageReceived += CheckForSpam;
	}

	/// <summary>
	///     Check if the message is spam.
	/// </summary>
	/// <param name="message">Received Message</param>
	/// <returns></returns>
	private async Task CheckForSpam(SocketMessage message)
	{
		if (string.IsNullOrEmpty(message.Content) || message.Author.Id == Client.CurrentUser.Id) return;

		var channel = (SocketTextChannel) message.Channel;
		var guild = channel.Guild;
		var author = message.Author;
		var user = (SocketGuildUser) author;

		var config = AntiSpamConfigRepository.Get(guild.Id);

		if (config == null)
		{
			Log.Debug("Could not find an AntiSpam Config for {ClientName} on {GuildName}", BotName, guild.Name);
			return;
		}

		if (config.IgnorePrefixes != null)
			try
			{
				var prefixes = config.IgnorePrefixes.Split(",").Select(x => x.Trim());

				if (!prefixes.Any(x => message.Content.StartsWith(x)))
					_messages.Insert(message, guild);
			}
			catch
			{
				Log.Error("Could not split prefixes in AntiSpamConfig with ID {ConfigId}", config.Id);
			}

		else _messages.Insert(message, guild);

		var queue = _messages.GetQueue(message, guild);

		if (ContainsSpam(queue))
			if (_spamCleaners.Any(x => x.User == user))
			{
				_spamCleaners.First(x => x.User == user).Restart();
				Log.Debug("Restarted Handler for {User}", user.Nickname);
			}
			else
			{
				var handler = new SpamCleaner(_lo)
			}
	}

	/// <summary>
	///     Checks if a given message queue contains spam.
	/// </summary>
	/// <param name="queue"></param>
	/// <returns>True if spam was found, else false</returns>
	private static bool ContainsSpam(SocketMessageQueue queue)
	{
		return queue.Queue.GroupBy(message => message.Content).Any(group => group.Count() > 5);
	}
}