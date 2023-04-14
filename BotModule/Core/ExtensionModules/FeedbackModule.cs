using BotModule.Extensions.Feedback;
using Discord;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private async Task LoadFeedbackModule(IGuild guild)
    {
        LogStartupAction($"Loaded Feedback Module for {Name} in {guild.Name}");

        var feedbackModule = _interactionService.Modules.First(x => x.Name == nameof(FeedbackExtension));

        _modules.Add(feedbackModule);
    }
}