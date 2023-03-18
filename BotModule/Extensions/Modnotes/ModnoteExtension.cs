using System.Text;
using BotModule.DI;
using BotModule.Extensions.Logging;
using DB.Models.Objects;
using DB.Repositories;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Strings.Manager;

namespace BotModule.Extensions.Modnotes;

public class ModnoteExtension : InteractionModuleBase
{
    public enum ModnoteCommandType
    {
        Show,
        Add,
        Remove
    }

    private readonly ILanguageProvider _languageProvider;
    private readonly ILoggingProvider _loggingProvider;
    private readonly IModuleState _moduleState;

    public ModnoteExtension(ILanguageProvider languageProvider, IModuleState moduleState,
        ILoggingProvider loggingProvider)
    {
        _languageProvider = languageProvider;
        _moduleState = moduleState;
        _loggingProvider = loggingProvider;
    }

    [DefaultMemberPermissions(GuildPermission.ManageMessages)]
    [SlashCommand("modnotes", "Add, Show, or Remove Modnotes.")]
    public async Task ModnoteCommand(SocketGuildUser user, ModnoteCommandType type, int id = 0)
    {
        var config = ModnoteConfigRepository.Get(user.Guild.Id);

        if (config is not { IsEnabled: true })
        {
            await RespondAsync(
                _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Modnote, "error_not_enabled"));
            return;
        }

        switch (type)
        {
            case ModnoteCommandType.Add:
                var guid = Guid.NewGuid();
                _moduleState.ModnoteCommandState.Add(guid, user);
                await RespondWithModalAsync<ModnoteModal>($"add_modnote:{guid}");
                break;

            case ModnoteCommandType.Show:
                await ShowModnote(user, id);
                break;

            case ModnoteCommandType.Remove:
                await DeleteModnote(user, id);
                break;
        }
    }

    [ModalInteraction("add_modnote:*")]
    public async Task AddModnote(string id, ModnoteModal modal)
    {
        var guid = Guid.Parse(id);

        if (_moduleState.ModnoteCommandState.TryGetValue(guid, out var user))
            if (!string.IsNullOrEmpty(modal.Modnote))
            {
                ModnoteRepository.Create(user.Guild.Id, user.Id, Context.User.Id, modal.Modnote);
                var text = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Modnote,
                    "modnote_created");
                text = string.Format(text, user.GetDiscriminatedUser(), user.Id);

                await RespondAsync(text);
                await LogModnote(user, modal.Modnote);
            }
    }

    private async Task DeleteModnote(SocketGuildUser user, int id)
    {
        throw new NotImplementedException();
    }

    private async Task ShowModnote(SocketGuildUser user, int id)
    {
        if (id > 0)
        {
            var emb = GetModNote(user, id);

            if (emb != null)
            {
                await RespondAsync(" ", embed: emb);
            }
            else
            {
                var text = _languageProvider.GetResource(
                    Context,
                    ResourceLookup.ResourceGroup.Modnote,
                    "error_id_mismatch");

                text = string.Format(text, id, user.GetDiscriminatedUser());

                await RespondAsync(text);
            }
        }
        else
        {
            await RespondAsync("", embed: GetModNotes(user));
        }
    }

    private Embed GetModNotes(SocketGuildUser user)
    {
        var notes = ModnoteRepository.Get(user.Guild.Id, user.Id);

        var embedBuilder = new EmbedBuilder();

        var modNotes = notes as Modnote[] ?? notes.ToArray();

        if (modNotes.Any())
        {
            var sb = new StringBuilder();

            for (var i = 0; i < modNotes.Length; i++)
            {
                var note = modNotes.ElementAt(i);
                if (note.Note.Length > 15)
                    sb.AppendLine($"{i + 1} - {note.Note[..15]}...");
                else
                    sb.AppendLine($"{i + 1} - {note.Note}...");
            }

            var title = _languageProvider.GetResource(
                Context,
                ResourceLookup.ResourceGroup.Modnote,
                "modnote_overview_title");

            title = string.Format(title, user.GetDiscriminatedUser());

            embedBuilder.WithTitle(title).AddField("Modnotes", sb.ToString());
        }
        else
        {
            embedBuilder.WithTitle($"{user} has no ModNotes!");
        }

        return embedBuilder.Build();
    }

    private Embed? GetModNote(SocketGuildUser user, int id)
    {
        var note = ModnoteRepository.Get(user.Guild.Id, user.Id, id);

        if (note == null) return null;

        var author = Context.Guild.GetUserAsync(note.Author.Snowflake).Result;

        var title = _languageProvider.GetResource(
            Context,
            ResourceLookup.ResourceGroup.Modnote,
            "modnote_view_title");

        title = string.Format(title, id, user.GetDiscriminatedUser(), author.GetDiscriminatedUser(), note.DateLogged);

        var eb = new EmbedBuilder().WithTitle(title);
        eb.AddField("Note", note.Note);

        return eb.Build();
    }

    private async Task LogModnote(SocketGuildUser user, string note)
    {
        var logger = _loggingProvider.Retrieve((DiscordSocketClient)Context.Client, Context.Guild);
        if (logger == null) return;

        var loggingInfo = new Dictionary<string, string>();

        var title = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Modnote, "modnote_log_title");

        var message =
            _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Modnote, "modnote_log_message");

        message = string.Format(message, Context.User.GetDiscriminatedUser(), Context.User.Id,
            user.GetDiscriminatedUser(),
            user.Id);

        var titleTwo =
            _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Modnote, "modnote_log_message_title");

        loggingInfo.Add(title, message);
        loggingInfo.Add(titleTwo, note);

        await logger.LogToDefaultChannel(" ", EmbedGenerator.GenerateLoggingEmbed(loggingInfo),
            Context.Guild.Id);
    }

    public class ModnoteModal : IModal
    {
        [InputLabel("Note Text")]
        [ModalTextInput("add_modnote", TextInputStyle.Paragraph, "Naughty Actions Here")]
        public string? Modnote { get; set; }

        public string Title => "Add Modnote";
    }
}