using Discord;
using Strings.Manager;
using Strings.Util;

namespace BotModule.DI;

public interface ILanguageProvider
{
	public List<LanguageInfo> LanguageInfos { get; set; }

	/// <summary>
	///     Registers a new LanguageInfo combination.
	/// </summary>
	/// <param name="info"></param>
	public void Register(LanguageInfo info);

	/// <summary>
	///     Gets a resource based on the current combination.
	/// </summary>
	/// <param name="guildId">Guild Id</param>
	/// <param name="clientId">Client (Bot) Id</param>
	/// <param name="group">Resource group to import</param>
	/// <param name="resourceKey">Resource key to lookup</param>
	/// <returns></returns>
	public string GetResource(ulong guildId, ulong clientId, ResourceLookup.ResourceGroup group, string resourceKey);

	/// <summary>
	///     Gets a resource based on the current combination.
	/// </summary>
	/// <param name="context">The interaction context</param>
	/// <param name="group">Resource group to import</param>
	/// <param name="resourceKey">Resource key to lookup</param>
	/// <returns></returns>
	public string GetResource(IInteractionContext context, ResourceLookup.ResourceGroup group, string resourceKey);
}