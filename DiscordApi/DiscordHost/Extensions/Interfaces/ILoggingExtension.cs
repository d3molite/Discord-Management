using Discord;
using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Extensions.Interfaces;

public interface ILoggingExtension
{
    DiscordSocketClient Client { get; set; }
    
    void LogSetup();

    void Pause();

    void Resume();

    Embed GenerateEmbed(string title, string message, Color? color = null, List<IAttachment>? attachments = null);

    Task SendLogMessage(string message, Embed embed, ulong channelId);

    Task SendCustomLogMessage(string message, Embed embed, ulong channelId);

    public Task SendLogToDefaultChannel(string message, Embed embed, ulong guildId);
}