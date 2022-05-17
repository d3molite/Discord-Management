using System.Text;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApi.Data.Repositories;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Models;

namespace DiscordApi.DiscordHost.Extensions.Modnotes;

public class ModNoteCommandHandler : InteractionModuleBase
{
    public enum CommandTypes
    {
        Show,
        Add,
        Remove
    }

    private readonly IStateHandler _handler;

    public ModNoteCommandHandler(IStateHandler handler)
    {
        _handler = handler;
    }


    [SlashCommand("modnotes", "Add, Show or Remove ModNotes.")]
    public async Task ModNotesCommand(SocketGuildUser user, CommandTypes type, int id = 0)
    {
        switch (type)
        {
            case CommandTypes.Show:
                await HandleShow(user, id);
                break;

            case CommandTypes.Add:
                var guid = Guid.NewGuid();
                _handler.ModalCommandState.Add(guid, user);
                await RespondWithModalAsync<AddModNoteModal>($"add_modnote:{guid}");
                break;

            case CommandTypes.Remove:
                await HandleDelete(user, id);
                break;
        }
    }

    [ModalInteraction("add_modnote:*")]
    public async Task AddModNote(string id, AddModNoteModal modal)
    {
        var guid = Guid.Parse(id);

        if (_handler.ModalCommandState.TryGetValue(guid, out var user))
        {
            if (!string.IsNullOrEmpty(modal.ModNote))
            {
                ModNoteRepository.Create(user.Guild.Id, user.Id, Context.User.Id, modal.ModNote);
                await RespondAsync($"ModNote created for {user}");
            }
            else
            {
                await RespondAsync("ModNote was empty.");
            }

            _handler.ModalCommandState.Remove(guid);
        }
    }

    private async Task HandleDelete(SocketGuildUser user, int id = 0)
    {
        if (id >= 0)
        {
            if (ModNoteRepository.Delete(user.Guild.Id, user.Id, id))
            {
                await RespondAsync($"Deleted Note {id} for {user}");
            }

            else
            {
                await RespondAsync($"No ModNote at {id} for {user}");
            }
        }
        else
        {
            await RespondAsync("ModNote ID has to be provided.");
        }
    }

    private async Task HandleShow(SocketGuildUser user, int id = 0)
    {
        if (id > 0)
        {
            var emb = GetModNote(user, id);

            if (emb != null)
                await RespondAsync(" ", embed: emb);
            else
                await RespondAsync($"No ModNote at {id} for {user}");
        }
        else
        {
            await RespondAsync("", embed: GetModNotes(user));
        }
    }

    private static Embed GetModNotes(SocketGuildUser user)
    {
        var notes = ModNoteRepository.Get(user.Guild.Id, user.Id);

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

            embedBuilder.WithTitle($"ModNotes for {user}").AddField("Notes", sb.ToString());
        }
        else
        {
            embedBuilder.WithTitle($"{user} has no ModNotes!");
        }

        return embedBuilder.Build();
    }

    private Embed? GetModNote(SocketGuildUser user, int id)
    {
        var note = ModNoteRepository.Get(user.Guild.Id, user.Id, id);

        if (note == null) return null;

        var author = Context.Guild.GetUserAsync(note.Author.UserID).Result;

        var eb = new EmbedBuilder().WithTitle($"ModNote {id} for {user} - logged by {author} on {note.Logged}");
        eb.AddField("Note", note.Note);

        return eb.Build();
    }


    public class AddModNoteModal : IModal
    {
        [InputLabel("Modnote Text")]
        [ModalTextInput("add_modnote", TextInputStyle.Paragraph, "Naughty Actions Here")]
        public string? ModNote { get; set; }

        public string Title => "Add Modnote";
    }
}