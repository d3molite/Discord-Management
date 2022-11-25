using DB.Models;
using DB.Models.Configs;
using DB.Models.Configs.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class ApiDbContext : DbContext
{
	public ApiDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Bot> Bots { get; set; } = null!;
	public DbSet<GuildConfig> GuildConfigs { get; set; } = null!;

	public DbSet<FeedbackConfig> FeedbackConfigs { get; set; } = null!;

	public static ApiDbContext Get()
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
		optionsBuilder.UseSqlite("Data Source=ApiDb.db");
		return new ApiDbContext(optionsBuilder.Options);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<GuildConfig>()
			.Navigation(x => x.LinkedGuild)
			.AutoInclude();

		modelBuilder.Entity<GuildConfig>()
			.Navigation(x => x.FeedbackConfig)
			.AutoInclude();

		modelBuilder.Entity<GuildConfig>()
			.Navigation(x => x.FaqConfig)
			.AutoInclude();

		modelBuilder.Entity<FeedbackConfig>()
			.Navigation(x => x.TargetChannel)
			.AutoInclude();

		modelBuilder.Entity<FaqConfig>()
			.Navigation(x => x.FaqItems)
			.AutoInclude();

		modelBuilder.Entity<GuildChannel>()
			.Navigation(x => x.LinkedGuild)
			.AutoInclude();

		modelBuilder.Entity<Bot>()
			.Navigation(x => x.Configs)
			.AutoInclude();
	}
}