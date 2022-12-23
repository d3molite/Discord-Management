using BotModule.DI;
using BotModule.Extensions.Base;
using DB.Repositories;
using Discord;
using Discord.WebSocket;

namespace BotModule.Extensions.Logging;

public sealed partial class LoggingExtension : ClientExtension, ILoggingExtension
{
    private bool _active = true;
    private ILanguageProvider _languageProvider;

    public LoggingExtension(DiscordSocketClient client, string botName, ILanguageProvider languageProvider)
        : base(client, botName, languageProvider)
    {
        _languageProvider = languageProvider;
        client.MessageDeleted += LogMessageDeleted;
        client.UserBanned += LogUserBanned;
        client.UserLeft += LogUserLeft;
        client.UserJoined += LogUserJoined;
    }

    /// <inheritdoc />
    public async Task LogToChannel(string message, Embed embed, ulong channelId)
    {
        if (_active)
        {
            var logChannel = await Client.GetChannelAsync(channelId);

            if (logChannel is IMessageChannel msgChannel)
                await msgChannel.SendMessageAsync(message, embed: embed);
        }
    }

    /// <inheritdoc />
    public async Task ForceLogToChannel(string message, Embed embed, ulong channelId)
    {
        var logChannel = await Client.GetChannelAsync(channelId);

        if (logChannel is IMessageChannel msgChannel)
            await msgChannel.SendMessageAsync(message, embed: embed);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void Pause()
    {
        _active = false;
    }

    /// <inheritdoc />
    public void Resume()
    {
        _active = true;
    }
}