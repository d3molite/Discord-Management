namespace DB.Models;

public class FaqItem
{
	public int Id { get; set; }

	public string Triggers { get; set; } = null!;

	public string Response { get; set; } = null!;
}