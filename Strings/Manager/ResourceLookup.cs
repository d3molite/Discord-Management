using System.Globalization;
using System.Resources;
using Strings.Resources;

namespace Strings.Manager;

public static class ResourceLookup
{
	public enum ResourceGroup
	{
		Feedback
	}

	private static readonly ResourceManager _feedbackManager = new(typeof(FeedbackResources));

	public static string? GetResourceString(ResourceGroup group, string name, CultureInfo? culture)
	{
		return group switch
		{
			ResourceGroup.Feedback => _feedbackManager.GetString(name, culture),
			_ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
		};
	}
}