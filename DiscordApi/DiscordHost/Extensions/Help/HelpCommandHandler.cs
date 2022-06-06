using Discord.Interactions;

namespace DiscordApi.DiscordHost.Extensions.Help;

public class HelpCommandHandler : InteractionModuleBase
{
    [SlashCommand("help", "get help for all active functions.")]
    public async Task Help()
    {
    }
}