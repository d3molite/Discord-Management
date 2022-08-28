using System.Globalization;
using System.Resources;
using Discord;
using Discord.WebSocket;
using DiscordApi.Data;
using DiscordApi.Models;
using DiscordApi.Resources.Extensions;
using DiscordApi.Services;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.Logging;

public class GeneralLogger : LoggingExtension
{
    public GeneralLogger(DiscordSocketClient client, string botName)
        : base(client, botName)
    {
        LogSetup();

        Resources = new ResourceManager(typeof(LoggingResources));
    }

    // public ResourceManager Resources { get; set; }

    private new void LogSetup()
    {
        base.LogSetup();

        Client.MessageDeleted += OnMessageDeleted;
        Client.UserJoined += OnUserJoined;
        Client.UserBanned += OnUserBanned;
        Client.UserLeft += OnUserLeft;

        Log.Information("Logger Attached to {ClientName}", BotName);
    }

    private async Task OnMessageDeleted(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
    {
        if (message.HasValue)
        {
            if (message.Value.Author.Id == Client.CurrentUser.Id) return;

            Log.Debug("MessageDeleted triggered for {ClientName} in {ChannelName}", BotName,
                message.Value.Channel.Name);

            var guild = ((SocketGuildChannel)message.Value.Channel).Guild;

            var culture = new CultureInfo(LocalizationService.GetLocale(BotName, guild.Id));

            if (TryGetConfig<LoggingConfig>(guild.Id, out var config))
                if (config.LogMessageDeleted)
                {
                    var embTitle = GetResource(nameof(LoggingResources.message_deleted_title), culture)!;

                    var embMessage =
                        string.Format(GetResource(nameof(LoggingResources.message_deleted_text), culture)!,
                            GetDiscriminatedUser(message.Value.Author), message.Value.Author.Id, channel.Value.Id,
                            message.Value);

                    if (message.Value.Attachments.Count > 0)
                    {
                        await SendLogMessage(" ",
                            GenerateEmbed(embTitle, embMessage, Color.Orange,
                                message.Value.Attachments.ToList()),
                            config.LoggingChannelID);
                    }
                    else
                    {
                        await SendLogMessage(" ", GenerateEmbed(embTitle, embMessage, Color.Orange),
                            config.LoggingChannelID);
                    }
                }
        }
    }

    private string OffsetToDate(TimeSpan time, CultureInfo culture)
    {
        var age = Math.Round(time.TotalDays, 2);

        return age switch
        {
            // if the time is smaller than one day.
            < 1 => Math.Round(time.TotalHours, 2) + GetResource(nameof(LoggingResources.hours), culture),

            // if the time is smaller than a year.
            < 365 => age + GetResource(nameof(LoggingResources.days), culture),

            // if the time is greater than a year.
            _ => Math.Round(time.TotalDays / 365, 2) + GetResource(nameof(LoggingResources.years), culture)
        };
    }

    private async Task OnUserJoined(SocketGuildUser user)
    {
        var guild = user.Guild;
        Log.Debug("UserJoined triggered for {ClientName} in {GuildName}", BotName, guild.Name);

        var culture = new CultureInfo(LocalizationService.GetLocale(BotName, guild.Id));

        using var context = AppDBContext.Get();

        if (TryGetConfig<LoggingConfig>(guild.Id, out var config))
            if (config.LogUserJoined)
            {
                var ageString = OffsetToDate(DateTime.Now - user.CreatedAt, culture);

                var embTitle = GetResource(nameof(LoggingResources.user_joined_title), culture)!;

                var embMessage = string.Format(
                    GetResource(
                        nameof(LoggingResources.user_joined_text), culture)!,
                    GetDiscriminatedUser(user), user.Id, ageString);

                await SendLogMessage(" ", GenerateEmbed(embTitle, embMessage), config.LoggingChannelID);
            }
    }

    private async Task OnUserLeft(SocketGuild guild, SocketUser user)
    {
        Log.Debug("UserLeft triggered for {ClientName} in {GuildName}", BotName, guild.Name);

        if (TryGetConfig<LoggingConfig>(guild.Id, out var config))
        {
            if (config.LogUserLeft)
            {
                var culture = new CultureInfo(LocalizationService.GetLocale(BotName, guild.Id));

                var time = "";

                if (user is SocketGuildUser guildUser)
                {
                    time = OffsetToDate(guild.GetUser(guildUser.Id).JoinedAt!.Value.Offset, culture);
                }

                var embTitle = GetResource(nameof(LoggingResources.user_left_title), culture)!;

                var embMessage = string.Format(
                    GetResource(
                        nameof(LoggingResources.user_left_text), culture)!,
                    GetDiscriminatedUser(user),
                    user.Id);

                if (!string.IsNullOrEmpty(time))
                    embMessage +=
                        string.Format(GetResource(nameof(LoggingResources.user_left_duration), culture)!, time);

                try
                {
                    var ban = await guild.GetBanAsync(user);
                    if (ban == null)
                        await SendLogMessage(" ", GenerateEmbed(embTitle, embMessage), config.LoggingChannelID);
                }
                catch (Exception ex)
                {
                    Log.Error("{ClientName} could not fetch Bans from Server: {GuildName} \n {Exception}", BotName,
                        guild.Name, ex.Message);
                    await SendLogMessage(" ", GenerateEmbed(embTitle, embMessage), config.LoggingChannelID);
                }
            }
            else
            {
                Log.Debug("Logging for UserLeft was disabled. Skipping.");
            }
        }
    }

    private async Task OnUserBanned(SocketUser user, SocketGuild guild)
    {
        if (TryGetConfig<LoggingConfig>(guild.Id, out var config))
            if (config.LogUserBanned)
            {
                var culture = new CultureInfo(LocalizationService.GetLocale(BotName, guild.Id));

                var banReason = string.Empty;

                try
                {
                    banReason = guild.GetBanAsync(user).Result.Reason;
                }
                catch (Exception ex)
                {
                    Log.Error("{ClientName} could not fetch Bans from Server: {GuildName} \n {Exception}", BotName,
                        guild.Name, ex.Message);

                    banReason = GetResource(nameof(LoggingResources.user_banned_error), culture);
                }

                var embTitle = GetResource(nameof(LoggingResources.user_banned_title), culture);

                var embMessage =
                    string.Format(
                        GetResource(nameof(LoggingResources.user_banned_text), culture)!,
                        GetDiscriminatedUser(user), user.Id, banReason);


                await SendLogMessage(" ", GenerateEmbed(embTitle, embMessage, Color.Red), config.LoggingChannelID);
            }
    }
}