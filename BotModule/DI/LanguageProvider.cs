using System.Globalization;
using Discord;
using Serilog;
using Strings.Manager;
using Strings.Util;

namespace BotModule.DI;

/// <summary>
///     Language Provider which registers and stores guild, client, localization combinations
/// </summary>
public class LanguageProvider : ILanguageProvider
{
	public LanguageProvider()
	{
		LanguageInfos = new List<LanguageInfo>();
	}

	public List<LanguageInfo> LanguageInfos { get; set; }

	/// <inheritdoc />
	public void Register(LanguageInfo info)
	{
		if (!LanguageInfos.Any(x => x.GuildId == info.GuildId && x.BotId == info.BotId)) LanguageInfos.Add(info);
	}

	/// <inheritdoc />
	public string GetResource(ulong guildId, ulong clientId, ResourceLookup.ResourceGroup group, string resourceKey)
	{
		var info = LanguageInfos.FirstOrDefault(x => x.GuildId == guildId && x.BotId == clientId);

		CultureInfo? culture = null;

		if (info != null) culture = info.Culture;

		var resource = ResourceLookup.GetResourceString(group, resourceKey, culture);

		if (resource != null) return resource;
		Log.Error(
			"Could not fetch resource string from {ResourceGroup} with key {ResourceKey} and culture {CultureName}",
			group, resourceKey, culture);

		return string.Empty;
	}

	/// <inheritdoc />
	public string GetResource(IInteractionContext context, ResourceLookup.ResourceGroup group, string resourceKey)
	{
		var info = LanguageInfos.FirstOrDefault(x =>
			x.GuildId == context.Guild.Id && x.BotId == context.Client.CurrentUser.Id);

		CultureInfo? culture = null;

		if (info != null) culture = info.Culture;

		var resource = ResourceLookup.GetResourceString(group, resourceKey, culture);

		if (resource != null) return resource;
		Log.Error(
			"Could not fetch resource string from {ResourceGroup} with key {ResourceKey} and culture {CultureName}",
			group, resourceKey, culture);

		return string.Empty;
	}

	public static CultureInfo GetCultureFromString(string? resource)
	{
		switch (resource)
		{
			default:
				return CultureInfo.InvariantCulture;
			case "DE":
				return CultureInfo.GetCultureInfo("de-de");
		}
	}
}