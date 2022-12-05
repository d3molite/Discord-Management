using Discord.WebSocket;

namespace BotModule.Extensions.AntiSpam.Objects;

public class GuildMessageCache
{
	private readonly int _maxMessages;

	private readonly Dictionary<SocketGuild, UserMessageCache> _messages = new();

	public GuildMessageCache(int maxMessages)
	{
		_maxMessages = maxMessages;
	}

	/// <summary>
	///     Inserts a message into a message cache.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="guild"></param>
	public void Insert(SocketMessage message, SocketGuild guild)
	{
		var userMessageCache = GetOrCreate(guild);
		var userMessages = userMessageCache.GetOrCreate(message.Author);
		userMessages.Enqueue(message);
	}

	/// <summary>
	///     Gets a single users MessageQueue for this guild.
	/// </summary>
	/// <param name="message">Message to check the author for</param>
	/// <param name="guild">Guild</param>
	/// <returns>Existing or new MessageQueue for the User</returns>
	public SocketMessageQueue GetQueue(SocketMessage message, SocketGuild guild)
	{
		var messages = GetOrCreate(guild);
		return messages.GetOrCreate(message.Author);
	}

	/// <summary>
	///     Gets or creates a user message cache assigned to a specific guild.
	/// </summary>
	/// <param name="guild">Guild the message cache is assigned to.</param>
	/// <returns>An existing or a new user message cache.</returns>
	private UserMessageCache GetOrCreate(SocketGuild guild)
	{
		if (_messages.TryGetValue(guild, out var messages)) return messages;

		var userMessages = new UserMessageCache(_maxMessages);
		_messages.Add(guild, userMessages);

		return userMessages;
	}
}