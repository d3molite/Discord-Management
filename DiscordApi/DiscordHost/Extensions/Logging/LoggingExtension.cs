using Discord;
using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Interfaces;
using DiscordApi.Models;

namespace DiscordApi.DiscordHost.Extensions.Logging;

public class LoggingExtension : Extension, ILoggingExtension
{
    private bool _paused;

    public LoggingExtension(DiscordSocketClient client, string botName) : base(botName)
    {
        Client = client;
        BotName = botName;
    }

    public DiscordSocketClient Client { get; set; }
    public string BotName { get; set; }

    public void LogSetup()
    {
    }


    public async Task SendLogMessage(string message, Embed embed, ulong channelId)
    {
        if (!_paused)
        {
            var logChannel = await Client.GetChannelAsync(channelId);

            if (logChannel is IMessageChannel msgChannel)
                await msgChannel.SendMessageAsync(message, embed: embed);
        }
    }

    public async Task SendCustomLogMessage(string message, Embed embed, ulong channelId)
    {
        var logChannel = await Client.GetChannelAsync(channelId);

        if (logChannel is IMessageChannel msgChannel)
            await msgChannel.SendMessageAsync(message, embed: embed);
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Resume()
    {
        _paused = false;
    }

    public async Task SendLogToDefaultChannel(string message, Embed embed, ulong guildId)
    {
        if (!_paused)
            if (TryGetConfig<LoggingConfig>(guildId, out var config))
            {
                var logChannel = await Client.GetChannelAsync(config.LoggingChannelID);

                if (logChannel is IMessageChannel msgChannel)
                    await msgChannel.SendMessageAsync(message, embed: embed);
            }
    }

    public Embed GenerateEmbed(string title, string message, Color? color = null, List<IAttachment>? attachments = null)
    {
        var embed = new EmbedBuilder
        {
            Title = "Log - " + DateTime.Now.ToString("HH:mm:ss")
        };
        if (color != null) embed.Color = color;
        embed.AddField(title, message);

        if (attachments != null)
            embed.AddField(
                "Attachments",
                string.Join(Environment.NewLine, attachments.Select(x => x.Url.ToString())));

        return embed.Build();
    }
}