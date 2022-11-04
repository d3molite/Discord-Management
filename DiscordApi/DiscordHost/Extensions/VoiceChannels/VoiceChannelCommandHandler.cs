using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordApi.Data.Repositories;
using DiscordApi.DiscordHost.Utils;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.VoiceChannels;

public class VoiceChannelCommandHandler : InteractionModuleBase
{
    private readonly IStateHandler _handler;

    public VoiceChannelCommandHandler(IStateHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("voice", "Create a new VoiceChannel")]
    public async Task Voice()
    {
        var channel = Context.Channel;
        var guild = Context.Guild;

        if (guild == null)
        {
            await RespondAsync("You must use this command in a guild!");
            return;
        }

        var config = VoiceConfigRepository.Get(guild.Id);

        if (config == null)
        {
            await RespondAsync("Voice Channels are not configured for this guild.");
            return;
        }

        if (config.OnlyAllowedIn != null && config.OnlyAllowedIn != channel.Id)
        {
            await RespondAsync($"This command can only be used in <#{config.OnlyAllowedIn}>");
            return;
        }

        await RespondWithModalAsync<CreateVoiceModal>($"add_channel:{Guid.NewGuid()}");
    }

    [ModalInteraction("add_channel:*")]
    public async Task CreateChannel(string id, CreateVoiceModal modal)
    {
        await DeferAsync();
        await CreateVoiceChannel(modal);
        await FollowupAsync("Channel has been created!");
    }

    private async Task CreateVoiceChannel(CreateVoiceModal modal)
    {
        var guild = Context.Guild;
        var config = VoiceConfigRepository.Get(guild.Id);
        var botId = ((DiscordSocketClient)Context.Client).CurrentUser.Id;

        var currentChannels =
            _handler.VoiceChannelStates.Where(x => x.BotId == botId);

        var category = await guild.GetChannelAsync(config!.CategoryId) as SocketCategoryChannel;

        var maxUsers = 0;

        var count = currentChannels.Count() + 1;

        var channelName = $"[{count}] {modal.ChannelName}";

        if (string.IsNullOrEmpty(modal.MaxUsers)) ;
        else maxUsers = int.Parse(modal.MaxUsers);

        var channel = await guild.CreateVoiceChannelAsync(
            channelName,
            delegate(VoiceChannelProperties properties)
            {
                properties.CategoryId = config.CategoryId;
                if (maxUsers > 0) properties.UserLimit = maxUsers;
            });

        var id = channel.Id;

        var socketChannel = await guild.GetChannelAsync(id) as SocketVoiceChannel;

        _handler.VoiceChannelStates.Add(new VoiceChannelState
        {
            BotId = botId,
            UsersPresentInLastCheck = true,
            VoiceChannel = socketChannel!
        });

        Log.Debug("Channel {Channel} created by {BotName}", socketChannel.Name, Context.Client.CurrentUser);
    }

    public class CreateVoiceModal : IModal
    {
        [InputLabel("Channel Name")]
        [ModalTextInput("channel_name", TextInputStyle.Short, minLength: 3, maxLength: 14,
            placeholder: "Channel Name Here")]
        public string ChannelName { get; set; }

        [InputLabel("User Limit (Leave 0 for Unlimited)")]
        [ModalTextInput("channel_limit", TextInputStyle.Short, minLength: 0, maxLength: 1, placeholder: "0",
            initValue: "0")]
        public string? MaxUsers { get; set; }

        public string Title => "Create Voice Channel";
    }
}