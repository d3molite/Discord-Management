using BotModule.Extensions.Faq;
using Discord;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private async Task LoadFaqModule(IGuild guild)
    {
        Log.Information("Loaded Faq Module for {BotName} in {GuildName}", Name, guild.Name);

        var faqModule = _interactionService.Modules.First(x => x.Name == nameof(FaqExtension));
        _modules.Add(faqModule);
    }
}