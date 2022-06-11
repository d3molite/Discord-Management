namespace DiscordApi.Models;

public class BotConfig : IConfig<BotConfig>
{
    [NonSerialized] public static Dictionary<string, string> Configs = new()
    {
        { "Basic", "" },
        { "Logging", nameof(RelatedLogger) },
        { "Reaction Roles", nameof(RoleConfigs) },
        { "Anti Spam", nameof(AntiSpam) }
    };

    public int ID { get; set; }

    public Guild RelatedGuild { get; set; }

    public Bot RelatedBot { get; set; }

    /// <summary>
    ///     Returns a linked Instance of a <see cref="LoggingConfig" /> if logging is enabled.
    /// </summary>
    public LoggingConfig? RelatedLogger { get; set; }

    public AntiSpamConfig? AntiSpam { get; set; }

    public List<ReactionRoleConfig> RoleConfigs { get; set; }

    public bool ImageManipulationEnabled { get; set; }

    public bool ModnotesEnabled { get; set; }

    public bool ESportsEnabled { get; set; }

    public string LastCommitPosted { get; set; }
}