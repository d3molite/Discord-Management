using DB.Models.Configs.Extensions;
using DB.Models.Interfaces;

namespace DB.Models.Configs;

public class GuildConfig : IConfig<GuildConfig>
{
    public int Id { get; set; }

    public Guild LinkedGuild { get; set; } = null!;

    public AntiSpamConfig? AntiSpamConfig { get; set; }

    public FeedbackConfig? FeedbackConfig { get; set; }

    public FaqConfig? FaqConfig { get; set; }
    public LoggingConfig? LoggingConfig { get; set; }

    public ModnoteConfig? ModnoteConfig { get; set; }

    public VoiceConfig? VoiceConfig { get; set; }

    public MessageReactionConfig? MessageReactionConfig { get; set; }
    
    public List<ReactionRoleConfig>? ReactionRoleConfigs { get; set; }
}