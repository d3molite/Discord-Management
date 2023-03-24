using Discord;
using Discord.Interactions;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private readonly IList<ModuleInfo> _modules = new List<ModuleInfo>();

    private async Task RegisterModules(IGuild guild)
    {
        await _interactionService.AddModulesToGuildAsync(guild, true, _modules.ToArray());

        Log.Debug("Added Commands for {BotName} to {GuildName}", Name, guild.Name);
    }
}