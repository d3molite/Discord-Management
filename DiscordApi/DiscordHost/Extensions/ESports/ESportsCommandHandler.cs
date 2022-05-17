using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using DiscordApi.DiscordHost.Utils;

namespace DiscordApi.DiscordHost.Extensions.ESports;

public class ESportsCommandHandler : InteractionModuleBase
{
    private readonly IStateHandler _handler;

    public ESportsCommandHandler(IStateHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("ready", "Initiate a ready check for all current voice users.")]
    public async Task ReadyCheck(string message)
    {
        SocketGuildUser user;

        try
        {
            user = Context.User as SocketGuildUser;
        }
        catch (InvalidCastException ex)
        {
            await RespondAsync("Command must be used in a Guild!");
            return;
        }

        if (!await CheckForVoice(user!))
        {
            return;
        }

        var guid = Guid.NewGuid();

        var builder = new ComponentBuilder()
            .WithButton(
                new ButtonBuilder("Ready", $"ready:{guid},yes"))
            .WithButton(
                new ButtonBuilder("Not Ready", $"ready:{guid},no"));

        var state = new ReadyCheckState(user!.VoiceChannel.Users.ToList(), message);
        state.Cooldown.Elapsed += async (sender, args) => await ConcludeReadyCheck(guid, state);

        _handler.ESportsCommandState.Add(
            guid,
            state);

        var sent = await ReplyAsync(" ", embed: ESportsCommandHelper.CreateEmbed(_handler.ESportsCommandState[guid]),
            components: builder.Build());

        state.Message = sent as RestUserMessage;

        await RespondAsync("Ready Check Initiated - 30 Seconds");
    }

    [ComponentInteraction("ready:*,*")]
    public async Task ReadyCheckResponse(string id, string choice)
    {
        var user = Context.User as SocketGuildUser;

        if (!await CheckForVoice(user!))
        {
            return;
        }

        var guid = Guid.Parse(id);

        var ch = choice switch
        {
            "no" => 0,
            "yes" => 1,
            _ => 0
        };

        _handler.ESportsCommandState.TryGetValue(guid, out var state);
        {
            state!.UpdateState(user!, ch);
            await state!.Message.ModifyAsync(x => x.Embed = ESportsCommandHelper.CreateEmbed(state));
        }

        await RespondAsync("", ephemeral: true);
    }

    private async Task ConcludeReadyCheck(Guid guid, ReadyCheckState state)
    {
        state.FinalizeStates();
        await state!.Message.ModifyAsync(x => x.Embed = ESportsCommandHelper.CreateEmbed(state));

        if (state.Success())
        {
            await state!.Message.Channel.SendMessageAsync("Ready Check Succeeded! ✅");
        }
        else
        {
            await state!.Message.Channel.SendMessageAsync(
                "Ready Check Failed! ❌");
        }

        await state!.Message.ModifyAsync(o => { o.Components = new ComponentBuilder().Build(); });

        _handler.ESportsCommandState.Remove(guid);
    }

    private async Task<bool> CheckForVoice(SocketGuildUser user)
    {
        if (user.VoiceChannel != null) return true;
        await RespondAsync("You must be in a voice channel!", ephemeral: true);
        return false;
    }
}