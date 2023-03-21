using BotModule.DI;
using BotModule.Extensions.Base;
using DB.Models.Configs.Extensions;
using DB.Repositories;
using Discord;
using Discord.WebSocket;

namespace BotModule.Extensions.MessageReaction;

public class MessageReactionExtension : ClientExtension
{
    private readonly Random _random = new();

    public MessageReactionExtension(DiscordSocketClient client, string botName, ILanguageProvider languageProvider)
        : base(client, botName, languageProvider)
    {
        client.MessageReceived += ReactToMessage;
    }

    private async Task ReactToMessage(SocketMessage message)
    {
        if (string.IsNullOrEmpty(message.Content) || message.Author.Id == Client.CurrentUser.Id) return;

        var channel = (SocketTextChannel)message.Channel;
        var guild = channel.Guild;

        var config = MessageReactionConfigRepository.Get(guild.Id);

        if (config is null) return;

        var matchingReactionItems = MatchContent(config, message.Content);

        if (!matchingReactionItems.Any()) return;

        var chance = _random.Next(0, 100);

        var matchingItem = GetRollMatches(matchingReactionItems, chance);

        switch (matchingItem)
        {
            case null:
                return;

            case { EmojiOnly: true, ReactionEmoji: { } }:
                await message.AddReactionAsync(Emoji.Parse(matchingItem.ReactionEmoji.EmojiString));
                break;

            default:
                await channel.SendMessageAsync(matchingItem.ReactionMessage);
                break;
        }
    }

    private static MessageReactionItem[] MatchContent(MessageReactionConfig config, string message)
    {
        return config.MessageReactions.Where(
            x => message.ToLower().Contains(x.ReactionTrigger)).ToArray();
    }

    private static MessageReactionItem? GetRollMatches(IEnumerable<MessageReactionItem> items, int roll)
    {
        return items.Where(x => roll <= x.ReactionChance)
            .MinBy(x => x.ReactionChance);
    }
}