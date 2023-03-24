using Discord;
using Discord.Interactions;

namespace BotModule.Extensions.ImageManipulation;

public partial class ImageCommandHandler
{
    [SlashCommand("undo", "Fuck, go back!")]
    public async Task undo()
    {
        var channel = Context.Channel;
        var msgs = await channel.GetMessagesAsync(10).FlattenAsync();
        var messages = msgs.ToList();

        foreach (var message in messages)
        {
            if (message.Author.Id != Context.Client.CurrentUser.Id) continue;
            await message.DeleteAsync();
            await RespondAsync("Done!", ephemeral: true);
            return;
        }
    }
}