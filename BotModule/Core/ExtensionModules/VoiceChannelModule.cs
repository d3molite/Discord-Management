using BotModule.DI;
using BotModule.Extensions.VoiceChannels;
using DB.Models.Configs.Extensions;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private VoiceChannelExtension _voiceChannelExtension;

    private async Task LoadVoiceChannelModule(IGuild guild, VoiceConfig config)
    {
        LogStartupAction($"Loaded Voice Channel Module for {Name} in {guild.Name}");

        var languageProvider = _serviceProvider.GetRequiredService<ILanguageProvider>();
        var moduleState = _serviceProvider.GetRequiredService<IModuleState>();

        _voiceChannelExtension =
            new VoiceChannelExtension(_botModel.Name, _client, languageProvider, moduleState, config);

        var voiceModule = _interactionService.Modules.First(x => x.Name == nameof(VoiceChannelCommandHandler));
        _modules.Add(voiceModule);
    }
}