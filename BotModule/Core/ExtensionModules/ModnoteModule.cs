using BotModule.Extensions.Modnotes;
using Discord;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private async Task LoadModnoteModule(IGuild guild)
    {
        LogStartupAction($"Loaded Modnote Module for {Name} in {guild.Name}");

        var modnoteModule = _interactionService.Modules.First(x => x.Name == nameof(ModnoteExtension));
        _modules.Add(modnoteModule);
    }
}