using Discord;
using Discord.Interactions;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private readonly IList<ModuleInfo> _modules = new List<ModuleInfo>();

    private async Task RegisterModules(IGuild guild)
    {
        await _interactionService.AddModulesToGuildAsync(guild, true, _modules.ToArray());

        LogStartupAction($"Added Commands for {Name} to {guild.Name}");
    }
}