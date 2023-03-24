using DB.Models.Interfaces;

namespace DB.Models;

public class Guild : DiscordItem
{
	public string? DefaultLanguage { get; set; }
}