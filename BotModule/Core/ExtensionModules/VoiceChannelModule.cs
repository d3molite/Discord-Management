using BotModule.DI;
using BotModule.Extensions.VoiceChannels;
using DB.Models.Configs.Extensions;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{

    private VoiceChannelExtension _voiceChannelExtension;
    
    private async Task LoadVoiceChannelModule(IGuild guild, VoiceConfig config)
    {
        Log.Information("Loaded Voice Channel Module for {BotName} in {GuildName}",
            Name, guild.Name);

        var languageProvider = _serviceProvider.GetRequiredService<ILanguageProvider>();
        var moduleState = _serviceProvider.GetRequiredService<IModuleState>();

        _voiceChannelExtension = new VoiceChannelExtension(_botModel.Name, _client, languageProvider, moduleState, config);

        var voiceModule = _interactionService.Modules.First(x => x.Name == nameof(VoiceChannelCommandHandler));
        await _interactionService.AddModulesToGuildAsync(guild, true, voiceModule);
    }
}