namespace DB.Models.Interfaces;

public class DiscordItem
{
    public int Id { get; set; }
    public ulong Snowflake { get; set; }

    public string? Name { get; set; }
}