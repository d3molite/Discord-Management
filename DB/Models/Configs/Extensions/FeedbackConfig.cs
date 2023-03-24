namespace DB.Models.Configs.Extensions;

public class FeedbackConfig
{
    public int Id { get; set; }

    public bool IsReactionsEnabled { get; set; }
    public GuildChannel TargetChannel { get; set; } = null!;
}