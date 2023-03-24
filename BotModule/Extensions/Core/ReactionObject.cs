using Discord;
using Discord.WebSocket;

namespace BotModule.Extensions.Core;

public class ReactionObject
{
	public ulong MessageId { get; }

	public ulong GuildId => Channel?.Guild.Id ?? 0;

	public IGuild? Guild => Channel?.Guild;
	
	public SocketGuildUser? User { get; }

	public SocketGuildChannel? Channel { get; }

	public string EmojiName { get; }

	private ReactionObject(
		ulong messageId,
		ulong authorId,
		SocketGuildChannel? channel,
		string emojiName)
	{
		MessageId = messageId;
		Channel = channel;
		EmojiName = emojiName;
		User = Channel?.Guild.GetUser(authorId);
	}

	public bool IsValid()
	{
		return Channel != null && !string.IsNullOrEmpty(EmojiName) && User != null;
	}

	public static async Task<ReactionObject> Create(
		Cacheable<IUserMessage, ulong> message,
		Cacheable<IMessageChannel, ulong> channel,
		SocketReaction reaction)
	{
		return new ReactionObject(
			message.Id,
			reaction.UserId,
			await channel.GetOrDownloadAsync() as SocketGuildChannel,
			reaction.Emote.Name);
	}
}