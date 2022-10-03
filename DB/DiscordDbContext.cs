using DB.Models;
using DB.Models.Configs;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class DiscordDbContext : DbContext
{
    public DiscordDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Bot> Bots { get; set; }

    public static DiscordDbContext Get()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DiscordDbContext>();
        optionsBuilder.UseSqlite("Data Source=Database.db");
        return new DiscordDbContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuildConfig>()
            .Navigation(x => x.LinkedGuild)
            .AutoInclude();

        modelBuilder.Entity<Bot>()
            .Navigation(x => x.Configs)
            .AutoInclude();
    }
}