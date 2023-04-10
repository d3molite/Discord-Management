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
        Logging,
        Modnote,
        Voice,
        Faq
    }

    private static readonly ResourceManager FeedbackManager = new(typeof(FeedbackResources));

    private static readonly ResourceManager AntiSpamManager = new(typeof(AntiSpamResources));

    private static readonly ResourceManager LoggingManager = new(typeof(LoggingResources));

    private static readonly ResourceManager ModnoteManager = new(typeof(ModnoteResources));

    private static readonly ResourceManager VoiceManager = new(typeof(VoiceResources));

    private static readonly ResourceManager FaqManager = new(typeof(FaqResources));

    public static string? GetResourceString(ResourceGroup group, string name, CultureInfo? culture)
    {
        return group switch
        {
            ResourceGroup.Feedback => FeedbackManager.GetString(name, culture),
            ResourceGroup.AntiSpam => AntiSpamManager.GetString(name, culture),
            ResourceGroup.Logging => LoggingManager.GetString(name, culture),
            ResourceGroup.Modnote => ModnoteManager.GetString(name, culture),
            ResourceGroup.Voice => VoiceManager.GetString(name, culture),
            ResourceGroup.Faq => FaqManager.GetString(name, culture),
            _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
        };
    }
}