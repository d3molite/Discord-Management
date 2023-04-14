using BotModule.DI;
using DB.Repositories;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Strings.Manager;

namespace BotModule.Extensions.VoiceChannels;

public class VoiceChannelCommandHandler : InteractionModuleBase
{
    private readonly ILanguageProvider _languageProvider;
    private readonly IModuleState _moduleState;

    public VoiceChannelCommandHandler(
        IModuleState moduleState,
        ILanguageProvider languageProvider)
    {
        _moduleState = moduleState;
        _languageProvider = languageProvider;
    }

    [SlashCommand("voice", "Create a new VoiceChannel")]
    public async Task Voice()
    {
        var channel = Context.Channel;
        var guild = Context.Guild;

        if (guild is null)
        {
            await RespondAsync(
                _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Voice, "error_guild"));
            return;
        }

        var config = VoiceConfigRepository.Get(guild.Id);

        if (config is null)
        {
            await RespondAsync(
                _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Voice, "error_config"));
            return;
        }

        if (config.RestrictedChannel is not null && config.RestrictedChannel.Snowflake != channel.Id)
        {
            var message = _languageProvider.GetResource(
                Context, ResourceLookup.ResourceGroup.Voice, "error_channel");
            message = string.Format(message, config.RestrictedChannel.Snowflake);

            await RespondAsync(message, ephemeral: true);
        }

        await RespondWithModalAsync<CreateVoiceModal>($"add_channel:{Guid.NewGuid()}");
    }

    [ModalInteraction("add_channel:*")]
    public async Task CreateChannel(string id, CreateVoiceModal modal)
    {
        await DeferAsync();
        var channel = await CreateVoiceChannel(modal);

        var message = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Voice,
            "voice_created");

        message = string.Format(message, channel.Name, channel.Id).Replace("\\n", Environment.NewLine);

        await FollowupAsync(message);
    }

    private async Task<IVoiceChannel> CreateVoiceChannel(CreateVoiceModal modal)
    {
        var guild = Context.Guild;
        var config = VoiceConfigRepository.Get(guild.Id);
        var botId = ((DiscordSocketClient)Context.Client).CurrentUser.Id;

        var currentChannels =
            _moduleState.VoiceChannelStates.Where(x => x.BotId == botId);

        var maxUsers = 0;

        var count = currentChannels.Select(x => ChannelToNumber(x.Channel.Name)).Max() + 1;

        var channelName = $"[{count}] {modal.ChannelName}";

        if (string.IsNullOrEmpty(modal.MaxUsers)) ;
        else maxUsers = int.Parse(modal.MaxUsers);

        var channel = await guild.CreateVoiceChannelAsync(
            channelName,
            delegate(VoiceChannelProperties properties)
            {
                properties.CategoryId = config!.Category.Snowflake;
                if (maxUsers > 0) properties.UserLimit = maxUsers;
            });

        var socketChannel = await guild.GetVoiceChannelAsync(channel.Id);

        _moduleState.VoiceChannelStates.Add(
            new VoiceChannelState
            {
                BotId = botId,
                UsersPresent = true,
                Channel = (socketChannel as SocketVoiceChannel)!
            });

        return channel;
    }
    
    public static int ChannelToNumber(string input)
    {
        return int.Parse(input.Split("]").First()[1..]);
    }

    public class CreateVoiceModal : IModal
    {
        [InputLabel("Channel Name")]
        [ModalTextInput("channel_name", minLength: 3, maxLength: 14,
            placeholder: "Channel Name Here")]
        public string ChannelName { get; set; } = string.Empty;

        [InputLabel("User Limit (Leave 0 for Unlimited)")]
        [ModalTextInput("channel_limit", minLength: 0, maxLength: 1, placeholder: "0",
            initValue: "0")]
        public string? MaxUsers { get; set; }

        public string Title => "Create Voice Channel";
    }
}