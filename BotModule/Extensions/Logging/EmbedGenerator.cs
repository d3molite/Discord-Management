using Discord;

namespace BotModule.Extensions.Logging;

public static class EmbedGenerator
{
    public static Embed GenerateLoggingEmbed(string title, string message, Color? color = null,
        List<IAttachment>? attachments = null)
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
                attachments);

        return embed.Build();
    }
}