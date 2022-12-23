using System.Globalization;
using System.Resources;
using Strings.Resources;

namespace Strings.Manager;

public static class ResourceLookup
{
    public enum ResourceGroup
    {
        Feedback,
        AntiSpam,
        Logging
    }

    private static readonly ResourceManager FeedbackManager = new(typeof(FeedbackResources));

    private static readonly ResourceManager AntiSpamManager = new(typeof(AntiSpamResources));

    private static readonly ResourceManager LoggingManager = new(typeof(LoggingResources));

    public static string? GetResourceString(ResourceGroup group, string name, CultureInfo? culture)
    {
        return group switch
        {
            ResourceGroup.Feedback => FeedbackManager.GetString(name, culture),
            ResourceGroup.AntiSpam => AntiSpamManager.GetString(name, culture),
            ResourceGroup.Logging => LoggingManager.GetString(name, culture),
            _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
        };
    }
}