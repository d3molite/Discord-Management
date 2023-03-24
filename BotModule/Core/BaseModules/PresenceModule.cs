using Discord;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    public async Task UpdatePresence(string? presence)
    {
        if (presence != null)
            await _client.SetActivityAsync(new Game(presence));
    }
}