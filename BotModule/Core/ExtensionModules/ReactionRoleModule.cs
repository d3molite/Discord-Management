using BotModule.DI;
using BotModule.Extensions.ReactionRoles;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
	private ReactionRoleExtension _reactionRoleExtension;
	
	private void LoadReactionRoleExtension(IGuild guild)
	{
		Log.Information("Loaded Reaction Role Module for {BotName} in {GuildName}",
			Name,
			guild.Name);

		_reactionRoleExtension = new ReactionRoleExtension(
			_client,
			Name,
			_serviceProvider.GetRequiredService<ILanguageProvider>());
	}
}