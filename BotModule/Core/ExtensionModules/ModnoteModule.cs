using BotModule.Extensions.Modnotes;
using Discord;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private async Task LoadModnoteModule(IGuild guild)
    {
        Log.Information("Loaded Modnote Module for {BotName} in {GuildName}", Name, guild.Name);

        var modnoteModule = _interactionService.Modules.First(x => x.Name == nameof(ModnoteExtension));
        await _interactionService.AddModulesToGuildAsync(guild, true, modnoteModule);
    }
}