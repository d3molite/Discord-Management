using DiscordApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordApi.Data;

public class AppDBContext : DbContext
{
    [NonSerialized] public static Dictionary<string, string> BaseItems = new()
    {
        { "Bots", nameof(Bots) },
        { "Guilds", nameof(Guilds) },
        { "Bot Configs", nameof(Configs) },
        { "Roles", nameof(Roles) },
        { "Emoji", nameof(Emoji) },
        { "Messages", nameof(Messages) }
    };

    public AppDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Bot> Bots { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Emoji> Emojis { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Guild> Guilds { get; set; }
    public DbSet<BotConfig> Configs { get; set; }
    public DbSet<Modnote> Modnotes { get; set; }

    public DbSet<Message> Messages { get; set; }
    public DbSet<SocialMediaConfig> SocialConfigs { get; set; }

    public static AppDBContext Get()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
        optionsBuilder.UseSqlite("Data Source=Database.db");
        return new AppDBContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Bot>()
            .Property(p => p.IsActive)
            .HasDefaultValue(false);

        modelBuilder
            .Entity<Bot>()
            .Property(p => p.IsDebug)
            .HasDefaultValue(false);

        modelBuilder
            .Entity<MessageReactionConfig>()
            .Navigation(x => x.MessageReactions)
            .AutoInclude();

        modelBuilder.Entity<MessageReaction>()
            .Navigation(x => x.ReactionEmoji)
            .AutoInclude();
    }
}