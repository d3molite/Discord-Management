using Discord.WebSocket;
using DiscordApi.Models;

namespace DiscordApi.DiscordHost.Extensions.Base;

public class ClientExtension : Extension
{
    public ClientExtension(string botName, DiscordSocketClient client)
        : base(botName)
    {
        Client = client;
    }

    public DiscordSocketClient Client { get; set; }

    protected async Task AddReaction(SocketGuild guild, SocketMessage message, Emoji emoji)
    {
        if (emoji.EmojiString.StartsWith("<") && emoji.EmojiString.EndsWith(">"))
        {
            await message.AddReactionAsync(Discord.Emote.Parse(emoji.EmojiString));
            return;
        }

        await message.AddReactionAsync(Discord.Emoji.Parse(emoji.EmojiString));
    }
}