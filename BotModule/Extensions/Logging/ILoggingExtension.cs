using BotModule.Extensions.Base;
using Discord;

namespace BotModule.Extensions.Logging;

public interface ILoggingExtension : IClientExtension
{
    /// <summary>
    ///     Log to a specific channel.
    /// </summary>
    /// <param name="message">Message Content.</param>
    /// <param name="embed">Embed.</param>
    /// <param name="channelId">Snowflake of channel to log to.</param>
    /// <returns></returns>
    public Task LogToChannel(string message, Embed embed, ulong channelId);

    /// <summary>
    ///     Force a log message, even if the logger is inactive.
    /// </summary>
    /// <param name="message">Message Content.</param>
    /// <param name="embed">Embed.</param>
    /// <param name="channelId">Snowflake of channel to log to.</param>
    /// <returns></returns>
    public Task ForceLogToChannel(string message, Embed embed, ulong channelId);

    /// <summary>
    ///     Logs to the default channel if one is specified in the config.
    /// </summary>
    /// <param name="message">Message Content.</param>
    /// <param name="embed"></param>
    /// <param name="guildId"></param>
    /// <returns></returns>
    public Task LogToDefaultChannel(string message, Embed embed, ulong guildId);

    /// <summary>
    ///     Pauses the logger.
    /// </summary>
    public void Pause();

    /// <summary>
    ///     Resumes the logger.
    /// </summary>
    public void Resume();
}