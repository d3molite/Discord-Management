using System.Globalization;
using System.Resources;
using Strings.Resources;

namespace Strings.Manager;

public static class ResourceLookup
{
    public enum ResourceGroup
    {
        Feedback,
        AntiSpam
    }

    private static readonly ResourceManager FeedbackManager = new(typeof(FeedbackResources));

    private static readonly ResourceManager AntiSpamManager = new(typeof(AntiSpamResources));

    public static string? GetResourceString(ResourceGroup group, string name, CultureInfo? culture)
    {
        return group switch
        {
            ResourceGroup.Feedback => FeedbackManager.GetString(name, culture),
            ResourceGroup.AntiSpam => AntiSpamManager.GetString(name, culture),
            _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
        };
    }
}