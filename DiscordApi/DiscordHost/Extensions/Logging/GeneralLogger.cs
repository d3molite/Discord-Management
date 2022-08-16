using Discord;
using Discord.WebSocket;
using DiscordApi.Data;
using DiscordApi.Models;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.Logging;

public class GeneralLogger : LoggingExtension
{
    public GeneralLogger(DiscordSocketClient client, string botName)
        : base(client, botName)
    {
        LogSetup();
    }

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

            if (TryGetConfig<LoggingConfig>(guild.Id, out var config))
                if (config.LogMessageDeleted)
                {
                    var discriminatedUser =
                        $"{message.Value.Author.Username}#{message.Value.Author.DiscriminatorValue}";
                    var embMessage =
                        $"Message by user {discriminatedUser} <@{message.Value.Author.Id}> \n was deleted in channel <#{channel.Value.Id}> \n **Content:** \n {message.Value}";

                    if (message.Value.Attachments.Count > 0)
                    {
                        await SendLogMessage(" ",
                            GenerateEmbed("Message Deleted", embMessage, Color.Orange,
                                message.Value.Attachments.ToList()),
                            config.LoggingChannelID);
                    }
                    else
                    {
                        await SendLogMessage(" ", GenerateEmbed("Message Deleted", embMessage, Color.Orange),
                            config.LoggingChannelID);
                    }
                }
        }
    }

    private string OffsetToDate(TimeSpan time)
    {
        var age = Math.Round(time.TotalDays, 2);

        return age switch
        {
            // if the time is smaller than one day.
            < 1 => Math.Round(time.TotalHours, 2) + " hours.",
            
            // if the time is smaller than a year.
            < 365 => age + " days.",
            
            // if the time is greater than a year.
            _ => Math.Round(time.TotalDays / 365, 2) + " years."
        };
    }

    private async Task OnUserJoined(SocketGuildUser user)
    {
        var guild = user.Guild;
        Log.Debug("UserJoined triggered for {ClientName} in {GuildName}", BotName, guild.Name);

        using var context = AppDBContext.Get();

        if (TryGetConfig<LoggingConfig>(guild.Id, out var config))
            if (config.LogUserJoined)
            {
                var ageString = OffsetToDate((DateTime.Now - user.CreatedAt));

                var discriminatedUser = $"{user.DisplayName}#{user.DiscriminatorValue}";
                var embMessage = $"User {discriminatedUser} <@{user.Id}> joined. \n **Account Age:** {ageString}";

                await SendLogMessage(" ", GenerateEmbed("User Joined", embMessage), config.LoggingChannelID);
            }
    }

    private async Task OnUserLeft(SocketGuild guild, SocketUser user)
    {
        Log.Debug("UserLeft triggered for {ClientName} in {GuildName}", BotName, guild.Name);

        if (TryGetConfig<LoggingConfig>(guild.Id, out var config))
        {
            if (config.LogUserLeft)
            {
                var discriminatedUser = $"{user.Username}#{user.DiscriminatorValue}";
                var time = "";

                if (user is SocketGuildUser guildUser) time = OffsetToDate(guildUser!.JoinedAt!.Value.Offset);
                
                var embMessage = $"User {discriminatedUser} <@{user.Id}> left.";
                
                if (!string.IsNullOrEmpty(time)) embMessage += $"\n **Time Spent:** {time}";

                try
                {
                    var ban = await guild.GetBanAsync(user);
                    if (ban == null)
                        await SendLogMessage(" ", GenerateEmbed("User Left", embMessage), config.LoggingChannelID);
                }
                catch (Exception ex)
                {
                    Log.Error("{ClientName} could not fetch Bans from Server: {GuildName} \n {Exception}", BotName,
                        guild.Name, ex.Message);
                    await SendLogMessage(" ", GenerateEmbed("User Left", embMessage), config.LoggingChannelID);
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
                var banReason = string.Empty;

                try
                {
                    banReason = guild.GetBanAsync(user).Result.Reason;
                }
                catch (Exception ex)
                {
                    Log.Error("{ClientName} could not fetch Bans from Server: {GuildName} \n {Exception}", BotName,
                        guild.Name, ex.Message);
                }

                var discriminatedUser = $"{user.Username}#{user.DiscriminatorValue}";
                var embMessage = $"User {discriminatedUser} <@{user.Id}> was banned. \n **Reason**: {banReason}";
                await SendLogMessage(" ", GenerateEmbed("User Banned", embMessage, Color.Red), config.LoggingChannelID);
            }
    }
}