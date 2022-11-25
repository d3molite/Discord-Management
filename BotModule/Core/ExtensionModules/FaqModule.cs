using BotModule.Extensions.Faq;
using Discord;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
	private async Task LoadFaqModule(IGuild guild)
	{
		Log.Information("Loaded Faq Module for {BotName} in {GuildName}", Name, guild.Name);

		var feedbackModule = _interactionService.Modules.First(x => x.Name == nameof(FaqExtension));

		await _interactionService.AddModulesToGuildAsync(guild, true, feedbackModule);
	}
}