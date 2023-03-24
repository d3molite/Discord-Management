using DB.Models;
using DB.Models.Base;
using DB.Models.Configs;
using DB.Models.Configs.Extensions;
using DB.Models.Objects;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Bot> Bots { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Guild> Guilds { get; set; } = null!;

    public DbSet<Emoji> Emojis { get; set; } = null!;
    public DbSet<Modnote> Modnotes { get; set; } = null!;
    public DbSet<GuildConfig> GuildConfigs { get; set; } = null!;
    public DbSet<FeedbackConfig> FeedbackConfigs { get; set; } = null!;

    public DbSet<AntiSpamConfig> AntiSpamConfigs { get; set; } = null!;

    public DbSet<ModnoteConfig> ModnoteConfigs { get; set; } = null!;

    public DbSet<VoiceConfig> VoiceConfigs { get; set; } = null!;

    public DbSet<ReactionRoleConfig> ReactionRoleConfigs { get; set; } = null!;


    public static ApiDbContext Get()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
        optionsBuilder.UseSqlite("Data Source=ApiDb.db");
        return new ApiDbContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CONFIGS
        // GUILD
        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.LinkedGuild)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.FeedbackConfig)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.FaqConfig)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.LoggingConfig)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.AntiSpamConfig)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.ModnoteConfig)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.VoiceConfig)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.MessageReactionConfig)
            .AutoInclude();

        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.ReactionRoleConfigs)
            .AutoInclude();

        // FEEDBACK
        modelBuilder.Entity<FeedbackConfig>()
            .Navigation(x => x.TargetChannel)
            .AutoInclude();

        // VOICE
        modelBuilder.Entity<VoiceConfig>()
            .Navigation(x => x.Category)
            .AutoInclude();

        modelBuilder.Entity<VoiceConfig>()
            .Navigation(x => x.RestrictedChannel)
            .AutoInclude();

        // MESSAGE REACTIONS
        modelBuilder.Entity<MessageReactionConfig>()
            .Navigation(x => x.MessageReactions)
            .AutoInclude();

        modelBuilder.Entity<MessageReactionItem>()
            .Navigation(x => x.ReactionEmoji)
            .AutoInclude();

        // FAQ
        modelBuilder.Entity<FaqConfig>()
            .Navigation(x => x.FaqItems)
            .AutoInclude();

        // ANTISPAM
        modelBuilder.Entity<AntiSpamConfig>()
            .Navigation(x => x.MutedRole)
            .AutoInclude();

        // LOGGING
        modelBuilder.Entity<LoggingConfig>()
            .Navigation(x => x.LoggingChannel)
            .AutoInclude();

        // REACTION ROLES
        modelBuilder.Entity<ReactionRoleConfig>()
            .Navigation(x => x.Message)
            .AutoInclude();

        modelBuilder.Entity<ReactionRoleConfig>()
            .Navigation(x => x.ReactionRoleItems)
            .AutoInclude();

        modelBuilder.Entity<ReactionRoleItem>()
            .Navigation(x => x.Role)
            .AutoInclude();

        modelBuilder.Entity<ReactionRoleItem>()
            .Navigation(x => x.Emoji)
            .AutoInclude();

        // OBJECTS
        modelBuilder.Entity<GuildChannel>()
            .Navigation(x => x.LinkedGuild)
            .AutoInclude();

        modelBuilder.Entity<Bot>()
            .Navigation(x => x.Configs)
            .AutoInclude();
    }
}