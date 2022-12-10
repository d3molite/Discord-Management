using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.Models;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.ReactTo;

public class MessageReactionExtension : ClientExtension
{
	private readonly Random _random = new();

	public MessageReactionExtension(DiscordSocketClient client, string botName) : base(botName, client)
	{
		BotName = botName;

		Client.MessageReceived += ReactToMessage;

		Log.Information("MessageReactions attached to {ClientName}", BotName);
	}

	private async Task ReactToMessage(SocketMessage message)
	{
		if (string.IsNullOrEmpty(message.Content) || message.Author.Id == Client.CurrentUser.Id) return;

		var channel = (SocketTextChannel) message.Channel;
		var guild = channel.Guild;
		var author = message.Author;

		if (TryGetConfig<MessageReactionConfig>(guild.Id, out var config))
		{
			var matches =
				config.MessageReactions.Where(x => message.Content.ToLower().Contains(x.ReactionTrigger));

			var messageReactions = matches as MessageReaction[] ?? matches.ToArray();

			if (!messageReactions.Any()) return;

			var chanceRoll = _random.Next(0, 100);

			Log.Debug("User {Author} rolled {Roll}", author.Username, chanceRoll);

			var match = messageReactions
				.Where(x => chanceRoll <= x.ReactionChance).MinBy(x => x.ReactionChance);

			if (match is null) return;

			if (match.EmojiOnly)
				try
				{
					await AddReaction(guild, message, match.ReactionEmoji!);
				}
				catch (Exception ex)
				{
					Log.Error("Could not add reaction with Emoji {Emoji}", match.ReactionEmoji);
					Log.Error("{EX}", ex.Message);
				}
			else
				await channel.SendMessageAsync(match.ReactionMessage);
		}
	}
}