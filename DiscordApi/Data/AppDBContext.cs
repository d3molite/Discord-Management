using DiscordApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordApi.Data;

public class AppDBContext : DbContext
{
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

    public static AppDBContext Get()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
        optionsBuilder.UseSqlite("Data Source=Database.db");
        return new AppDBContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}