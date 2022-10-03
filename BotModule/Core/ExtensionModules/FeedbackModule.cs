using Serilog;

namespace BotModule.Core.ExtensionModules;

public sealed class DiscordBot
{
    private async Task LoadFeedbackModule()
    {
        Log.Information("Loaded Feedback Module");
    }
}