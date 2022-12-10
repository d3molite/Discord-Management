using BotModule.Extensions.Base;
using DB.Repositories;
using Discord;
using Discord.WebSocket;

namespace BotModule.Extensions.Logging;

public class LoggingExtension : ClientExtension
{
	private bool _active = true;

	public LoggingExtension(DiscordSocketClient client, string botName)
		: base(client, botName)
	{
	}

	public async Task LogToChannel(string message, Embed embed, ulong channelId)
	{
		if (_active)
		{
			var logChannel = await Client.GetChannelAsync(channelId);

			if (logChannel is IMessageChannel msgChannel)
				await msgChannel.SendMessageAsync(message, embed: embed);
		}
	}

	public async Task ForceLogToChannel(string message, Embed embed, ulong channelId)
	{
		var logChannel = await Client.GetChannelAsync(channelId);

		if (logChannel is IMessageChannel msgChannel)
			await msgChannel.SendMessageAsync(message, embed: embed);
	}

	public async Task LogToDefaultChannel(string message, Embed embed, ulong guildId)
	{
		if (_active)
		{
			var config = LoggingConfigRepository.Get(guildId);

			if (config != null)
			{
				var logChannel = await Client.GetChannelAsync(config.LoggingChannel!.Snowflake);

				if (logChannel is IMessageChannel msgChannel)
					await msgChannel.SendMessageAsync(message, embed: embed);
			}
		}
	}

	public void Pause()
	{
		_active = false;
	}

	public void Resume()
	{
		_active = true;
	}
}