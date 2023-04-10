using BotModule.Extensions.ImageManipulation;
using Discord;
using Serilog;

namespace BotModule.Core;

public partial class DiscordBot
{
    private void LoadImageManipulationModule(IGuild guild)
    {
        Log.Information("Loaded Image Module for {BotName} in {GuildName}",
            Name, guild.Name);

        var imageExtension = _interactionService.Modules.First(x => x.Name == nameof(ImageCommandHandler));
        _modules.Add(imageExtension);
    }
}