namespace DB.Models.Configs;

public class GuildConfig : IConfig<GuildConfig>
{
    public int Id { get; set; }

    public Guild LinkedGuild { get; set; }
}