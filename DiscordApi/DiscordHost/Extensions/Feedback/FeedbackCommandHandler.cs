using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApi.Data;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Emoji = Discord.Emoji;

namespace DiscordApi.DiscordHost.Extensions.Feedback;

public class FeedbackCommandHandler : InteractionExtension
{
    private readonly IStateHandler _handler;

    public FeedbackCommandHandler(IStateHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("feedback", "Provide Feedback. Hit Enter and a message box will open where you can enter your text.")]
    public async Task FeedbackCommand()
    {
        var guid = Guid.NewGuid();
        _handler.ModalCommandState.Add(guid, (SocketGuildUser)Context.User);
        await RespondWithModalAsync<AddFeedbackModal>($"add_feedback:{guid}");
    }

    [ModalInteraction("add_feedback:*")]
    public async Task AddFeedback(string id, AddFeedbackModal modal)
    {
        var guid = Guid.Parse(id);

        if (_handler.ModalCommandState.TryGetValue(guid, out var user))
        {
            if (!string.IsNullOrEmpty(modal.Feedback))
            {
                if (TryGetConfig<FeedbackConfig>(Context.Guild.Id, Context.Client.CurrentUser.Username, out var config))
                {
                    var channel = await Context.Guild.GetChannelAsync(config.FeedbackChannelID);
                    
                    if (channel is IMessageChannel messageChannel)
                    {
                        if (Context.User is SocketGuildUser guildUser)
                        {
                            var message = await messageChannel.SendMessageAsync(" ", embed: CreateFeedbackEmbed(modal.Feedback, guildUser));
                            if (config.AddReactions)
                            {
                                await message.AddReactionsAsync(
                                    new List<IEmote>()
                                    {
                                        new Emoji("👍"),
                                        new Emoji("👎")
                                    });
                            }
                            
                            await RespondAsync("Feedback received. Thank you!", ephemeral: true);
                            return;
                        }
                        
                        await RespondAsync("Please use this command in a guild!", ephemeral: true);
                        return;
                    }
                }
                
                await RespondAsync("Something went wrong! Please contact a mod or administrator.");
                
                Log.Error("No Feedback Config for {BotName} in {Guild}", Context.Client.CurrentUser.Username, Context.Guild.Name);
            }
        }
    }

    private static IEnumerable<string> Split(string str, int chunkSize)
    {
        for (int index = 0; index < str.Length; index += chunkSize) {
            yield return str.Substring(index, Math.Min(chunkSize, str.Length - index));
        }
    }

    private static Embed CreateFeedbackEmbed(string feedbackText, SocketGuildUser user)
    {
        var embedBuilder = new EmbedBuilder();

        embedBuilder.Title = $"Feedback by user {user.DisplayName} at {DateTime.Now.ToString("dd.MM.yyyy ")}";

        var splitText = Split(feedbackText, 900);

        var iter = 1;

        var enumerable = splitText as string[] ?? splitText.ToArray();
        
        foreach (var text in enumerable)
        {
            embedBuilder.AddField($"{iter} / {enumerable.Length}", text);
            iter++;
        }

        return embedBuilder.Build();
    }

    public class AddFeedbackModal : IModal
    {
        [InputLabel("Feedback Text")]
        [ModalTextInput("add_feedback", TextInputStyle.Paragraph, "Your Feedback Here")]
        public string? Feedback { get; set; }

        public string Title => "Add Feedback";
    }
}