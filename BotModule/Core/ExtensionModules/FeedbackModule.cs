using BotModule.Extensions.Feedback;
using Discord;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private async Task LoadFeedbackModule(IGuild guild)
    {
        Log.Information("Loaded Feedback Module for {BotName} in {GuildName}", Name, guild.Name);

        var feedbackModule = _interactionService.Modules.First(x => x.Name == nameof(FeedbackExtension));

        await _interactionService.AddModulesToGuildAsync(guild, true, feedbackModule);
    }
}