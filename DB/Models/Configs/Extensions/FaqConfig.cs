namespace DB.Models.Configs.Extensions;

public class FaqConfig
{
	public int Id { get; set; }
	public List<FaqItem>? FaqItems { get; set; }
}