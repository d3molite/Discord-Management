/*using System.Text;
using Discord;
using DiscordApi.DiscordHost.Utils;

namespace DiscordApi.DiscordHost.Extensions.ESports;

public class ESportsCommandHelper
{
    public static Embed CreateEmbed(ReadyCheckState state)
    {
        var embedBuilder = new EmbedBuilder().WithTitle(state.Title)
            .WithFields(new EmbedFieldBuilder().WithName("Ready states").WithValue(CreateList(state)));

        return embedBuilder.Build();
    }

    private static string CreateList(ReadyCheckState state)
    {
        var users = state.ReadyStates;

        var sb = new StringBuilder();

        foreach (var user in users)
        {
            sb.AppendLine($"{user.Key.Username}: \t {Status(user.Value)}");
        }

        return sb.ToString();
    }

    private static string Status(int state)
    {
        return state switch
        {
            0 => "❌",
            1 => "✅",
            2 => "❓",
            _ => "BROKEN!"
        };
    }
}*/

