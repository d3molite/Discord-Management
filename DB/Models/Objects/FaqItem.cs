namespace DB.Models.Objects;

public class FaqItem
{
    public int Id { get; set; }
    public string Triggers { get; set; } = null!;

    public string Question { get; set; } = null!;

    public string Response { get; set; } = null!;

    public string? MessageLink { get; set; }
}