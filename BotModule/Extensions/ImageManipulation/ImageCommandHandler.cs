using BotModule.Extensions.ImageManipulation.Core;
using Discord;
using Discord.Interactions;

namespace BotModule.Extensions.ImageManipulation;

public class ImageCommandHandler : InteractionModuleBase
{
	private readonly List<string> _formats = new() { "jpg", "png", "bmp", "jpeg", "webp" };
	
	private string CleanImageUrl(string url)
	{
		return url.Split("?").First();
	}

	private bool IsImage(string url)
	{
		var imageExt = CleanImageUrl(url).Split('.').Last().ToLower();
		return _formats.Contains(imageExt);
	}

	private async Task<ImageResult> TryGetImage()
	{
		
	}

	private async Task<string?> GetImageLink()
	{
		var channel = Context.Channel;
		var messages = (await channel.GetMessagesAsync(10).FlattenAsync()).ToList();
		messages.Reverse();

		foreach (var message in messages)
		{
			
		}
	}
}