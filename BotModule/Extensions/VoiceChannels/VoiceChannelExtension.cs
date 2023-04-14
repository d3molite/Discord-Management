using BotModule.DI;
using BotModule.Extensions.Base;
using DB.Models.Configs.Extensions;
using DB.Repositories;
using Discord.WebSocket;
using Serilog;
using Strings.Manager;

namespace BotModule.Extensions.VoiceChannels;

public class VoiceChannelExtension : ClientExtension
{
    private readonly VoiceConfig _config;
    private readonly IModuleState _moduleState;
    private readonly PeriodicTimer _timer;
    private SocketTextChannel _loggingChannel;

    public VoiceChannelExtension(
        string botName,
        DiscordSocketClient client,
        ILanguageProvider languageProvider,
        IModuleState moduleState,
        VoiceConfig config) : base(client, botName, languageProvider)
    {
        _moduleState = moduleState;
        _config = config;

        _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        Task.Run(async () => await PollChannels());
    }

    private async Task PollChannels()
    {
        var guild = Client.GetGuild(_config.Category.LinkedGuild.Snowflake);
        var category = guild.GetCategoryChannel(_config.Category.Snowflake);

        Log.Debug("Polling Channels for {BotName} in {Guild}", BotName, guild.Name);

        if (_config.RestrictedChannel is not null)
        {
            _loggingChannel = await Client.GetChannelAsync(_config.RestrictedChannel!.Snowflake)
                as SocketTextChannel ?? throw new InvalidOperationException();
        }
        else
        {
            var config = LoggingConfigRepository.Get(guild.Id)!;

            _loggingChannel = await Client.GetChannelAsync(config.LoggingChannel.Snowflake)
                as SocketTextChannel ?? throw new InvalidOperationException();
        }

        Log.Debug("Logging for {BotName} in {Chanel}", BotName, _loggingChannel.Name);

        foreach (var channel in category.Channels)
            if (channel.Name.StartsWith("["))
                _moduleState.VoiceChannelStates.Add(new VoiceChannelState
                {
                    BotId = Client.CurrentUser.Id,
                    UsersPresent = true,
                    Channel = (channel as SocketVoiceChannel)!
                });

        foreach (var state in _moduleState.VoiceChannelStates)
            Log.Debug("Found {ChannelName} with {Bot}", state.Channel.Name, Client.CurrentUser.Username);

        Task.Run(async () => await RunPeriodicChecks());
    }

    private async Task RunPeriodicChecks()
    {
        while (await _timer.WaitForNextTickAsync(CancellationToken.None))
        {
            var removed = new List<VoiceChannelState>();

            foreach (var channelInfo in _moduleState.VoiceChannelStates.Where(x => x.BotId == Client.CurrentUser.Id))
            {
                var lastCheck = channelInfo.UsersPresent;
                var currentCheck = channelInfo.Channel.ConnectedUsers.Any();

                var creationChannel =
                    await Client.GetChannelAsync(_config.RestrictedChannel!.Snowflake) as SocketTextChannel;

                if (!lastCheck && !currentCheck)
                {
                    var message = GetResource(_config.Category.LinkedGuild.Snowflake,
                        ResourceLookup.ResourceGroup.Voice, "channel_deleted");

                    message = string.Format(message, channelInfo.Channel.Name);

                    await creationChannel!.SendMessageAsync(message);

                    await channelInfo.Channel.DeleteAsync();
                    removed.Add(channelInfo);

                    Log.Debug("Channel {ChannelName} deleted due to Inactivity by {BotName}", channelInfo.Channel.Name,
                        BotName);
                    continue;
                }

                switch (currentCheck)
                {
                    case true:
                        channelInfo.UsersPresent = true;
                        continue;
                    case false:
                        channelInfo.UsersPresent = false;
                        break;
                }
            }

            foreach (var channelInfo in removed) _moduleState.VoiceChannelStates.Remove(channelInfo);
        }
    }
}