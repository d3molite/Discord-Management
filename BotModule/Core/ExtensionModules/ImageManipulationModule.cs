using BotModule.Extensions.ImageManipulation;
using Discord;

namespace BotModule.Core;

public partial class DiscordBot
{
    private void LoadImageManipulationModule(IGuild guild)
    {
        LogStartupAction($"Loaded Image Module for {Name} in {guild.Name}");

        var imageExtension = _interactionService.Modules.First(x => x.Name == nameof(ImageCommandHandler));
        _modules.Add(imageExtension);
    }
}