using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Models;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.VoiceChannels;

public class VoiceChannelExtension : ClientExtension
{
    private readonly VoiceChannelConfig _config;
    private readonly IStateHandler _handler;
    private readonly PeriodicTimer _timer;

    public VoiceChannelExtension(string botName, DiscordSocketClient client, IStateHandler handler,
        VoiceChannelConfig config) : base(botName,
        client)
    {
        _handler = handler;
        _config = config;

        _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        Client.Ready += () => Task.Run(async () => await PollChannels());
        Task.Run(async () => await RunPeriodicChecks());
    }

    private async Task PollChannels()
    {
        Log.Debug("Polling Channels for {BotName}", BotName);

        var guild = Client.GetGuild(_config.VoiceGuild.GuildID);
        var category = guild.GetCategoryChannel(_config.CategoryId);

        foreach (var channel in category.Channels)
        {
            if (channel.Name.StartsWith("["))
            {
                _handler.VoiceChannelStates.Add(new VoiceChannelState
                {
                    BotId = Client.CurrentUser.Id,
                    UsersPresentInLastCheck = true,
                    VoiceChannel = (channel as SocketVoiceChannel)!
                });
            }
        }

        foreach (var channel in _handler.VoiceChannelStates)
        {
            Log.Debug("Found {ChannelName}", channel.VoiceChannel.Name);
        }
    }

    private async Task RunPeriodicChecks()
    {
        while (await _timer.WaitForNextTickAsync(CancellationToken.None))
        {
            if (_handler.VoiceChannelStates.Any())
            {
                Log.Debug("Checking... {BotName} - {Number} Channels", BotName, _handler.VoiceChannelStates.Count);
            }

            var removed = new List<VoiceChannelState>();

            foreach (var channelInfo in _handler.VoiceChannelStates)
            {
                var lastCheck = channelInfo.UsersPresentInLastCheck;
                var currentCheck = channelInfo.VoiceChannel.ConnectedUsers.Any();
                var creationChannel = await Client.GetChannelAsync(_config.OnlyAllowedIn!.Value) as SocketTextChannel;

                if (!lastCheck && !currentCheck)
                {
                    await creationChannel!.SendMessageAsync(
                        $"Channel **{channelInfo.VoiceChannel.Name}** has been deleted due to inactivity");
                    var name = channelInfo.VoiceChannel.Name;

                    await channelInfo.VoiceChannel.DeleteAsync();
                    removed.Add(channelInfo);

                    Log.Debug("Channel {ChannelName} deleted due to Inactivity by {BotName}", name, BotName);
                    continue;
                }

                switch (currentCheck)
                {
                    case true:
                        channelInfo.UsersPresentInLastCheck = true;
                        continue;
                    case false:
                        channelInfo.UsersPresentInLastCheck = false;
                        break;
                }
            }

            foreach (var channelInfo in removed)
            {
                _handler.VoiceChannelStates.Remove(channelInfo);
            }
        }
    }
}