using Discord.Interactions;

namespace BotModule.Extensions.Faq;

public class FaqExtension : InteractionModuleBase
{
	[SlashCommand("faq", "Enter a keyword and press enter!")]
	public async Task Faq(string keyword)
	{
	}
}