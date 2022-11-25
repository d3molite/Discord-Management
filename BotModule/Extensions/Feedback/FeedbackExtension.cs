using BotModule.DI;
using DB.Repositories;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Strings.Manager;

namespace BotModule.Extensions.Feedback;

public class FeedbackExtension : InteractionModuleBase
{
	private readonly ILanguageProvider _languageProvider;
	private readonly IModuleState _moduleState;

	public FeedbackExtension(IModuleState moduleState, ILanguageProvider languageProvider)
	{
		_moduleState = moduleState;
		_languageProvider = languageProvider;
	}

	private string GetResource(string resourceKey) =>
		_languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Feedback, resourceKey);

	[SlashCommand("feedback", "Just press enter.")]
	public async Task FeedbackCommand()
	{
		if (Context.User is SocketGuildUser guildUser)
		{
			var guid = Guid.NewGuid();
			_moduleState.FeedbackCommandState.Add(guid, guildUser);
			await RespondWithModalAsync<AddFeedbackModal>($"add_feedback:{guid}");
		}

		var response = GetResource("feedback_error_guild_required");
		await RespondAsync(response, ephemeral: true);
	}

	[ModalInteraction("add_feedback:*")]
	public async Task AddFeedback(string id, AddFeedbackModal modal)
	{
		var guid = Guid.Parse(id);

		if (_moduleState.FeedbackCommandState.TryGetValue(guid, out var user))
			if (!string.IsNullOrEmpty(modal.Feedback))
				if (FeedbackConfigRepository.TryGet(Context.Guild.Id, out var config))
				{
					var channel = await Context.Guild.GetChannelAsync(config!.TargetChannel.Snowflake);

					if (channel is IMessageChannel messageChannel)
					{
						var message =
							await messageChannel.SendMessageAsync(" ",
								embed: CreateFeedbackEmbed(modal.Feedback, user));

						if (config.IsReactionsEnabled)
							await message.AddReactionsAsync(
								new List<IEmote>
								{
									new Emoji("👍"),
									new Emoji("👎")
								});

						var response = GetResource("feedback_success");
						await RespondAsync(response, ephemeral: true);
					}
				}
	}

	private static IEnumerable<string> Split(string str, int chunkSize)
	{
		for (var index = 0; index < str.Length; index += chunkSize)
			yield return str.Substring(index, Math.Min(chunkSize, str.Length - index));
	}

	private Embed CreateFeedbackEmbed(string feedbackText, SocketGuildUser user)
	{
		var embedBuilder = new EmbedBuilder();

		var title = GetResource("feedback_received_title");
		embedBuilder.Title = string.Format(title, user.DisplayName, DateTime.Now.ToString("dd.MM.yyyy"));

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