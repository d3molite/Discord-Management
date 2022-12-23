using DB.Repositories;
using Discord;
using Discord.WebSocket;
using Serilog;
using Strings.Manager;

namespace BotModule.Extensions.Logging;

public sealed partial class LoggingExtension
{
    private string OffsetToDate(TimeSpan time, ulong guildId)
    {
        var age = Math.Round(time.TotalDays, 2);

        return age switch
        {
            // if the time is smaller than one day.
            < 1 => Math.Round(time.TotalHours, 2) + GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "hours"),

            // if the time is smaller than a year.
            < 365 => age + GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "days"),

            // if the time is greater than a year.
            _ => Math.Round(time.TotalDays / 365, 2) +
                 GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "years")
        };
    }

    private string GetResource(ulong guildId, string resourceKey)
    {
        return base.GetResource(guildId, ResourceLookup.ResourceGroup.Logging, resourceKey);
    }

    private async Task LogMessageDeleted(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
    {
        if (!_active) return;

        if (message.HasValue)
        {
            if (message.Value.Author.Id == Client.CurrentUser.Id) return;

            var guildId = ((SocketGuildChannel)message.Value.Channel).Guild.Id;

            var config = LoggingConfigRepository.Get(guildId);
            if (config == null) return;

            if (config.LogMessageDeleted)
            {
                var user = message.Value.Author;

                var embTitle = GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "message_deleted_title");
                var embMessage = GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "message_deleted_text");

                embMessage = string.Format(embMessage, user.GetDiscriminatedUser(), user.Id, channel.Id,
                    message.Value.Content);

                Embed embed;

                if (message.Value.Attachments.Count > 0)
                    embed = EmbedGenerator.GenerateLoggingEmbed(embTitle, embMessage, null,
                        message.Value.Attachments.ToList());
                else
                    embed = EmbedGenerator.GenerateLoggingEmbed(embTitle, embMessage);

                await LogToDefaultChannel(" ", embed, guildId);
            }
        }
    }

    private async Task LogUserBanned(SocketUser user, SocketGuild guild)
    {
        if (!_active) return;

        var guildId = guild.Id;
        string banReason;

        var config = LoggingConfigRepository.Get(guildId);
        if (config == null) return;

        try
        {
            banReason = guild.GetBanAsync(user).Result.Reason;
        }
        catch (Exception ex)
        {
            Log.Error("{ClientName} could not fetch Bans from Server: {GuildName} \n {Exception}",
                BotName,
                guild.Name,
                ex.Message);

            banReason = GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "user_banned_error");
        }

        var embTitle = GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "user_banned_title");
        var embMessage = GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "user_banned_text");

        embMessage = string.Format(embMessage, user.GetDiscriminatedUser(), user.Id, banReason);

        await LogToDefaultChannel(" ", EmbedGenerator.GenerateLoggingEmbed(embTitle, embMessage, Color.Red), guildId);
    }

    private async Task LogUserLeft(SocketGuild guild, SocketUser user)
    {
        if (!_active) return;

        var guildId = guild.Id;
        var config = LoggingConfigRepository.Get(guildId);

        if (config == null) return;

        if (config.LogUserLeft)
        {
            var embTitle = GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "user_left_title");
            var embMessage = GetResource(guildId, ResourceLookup.ResourceGroup.Logging, "user_left_text");

            embMessage = string.Format(embMessage, user.GetDiscriminatedUser(), user.Id);

            try
            {
                var ban = await guild.GetBanAsync(user);
                if (ban == null)
                    await LogToDefaultChannel(" ", EmbedGenerator.GenerateLoggingEmbed(embTitle, embMessage), guildId);
            }
            catch (Exception ex)
            {
                Log.Error("{ClientName} could not fetch Bans from Server: {GuildName} \n {Exception}", BotName,
                    guild.Name, ex.Message);
                await LogToDefaultChannel(" ", EmbedGenerator.GenerateLoggingEmbed(embTitle, embMessage), guildId);
            }
        }
    }

    private async Task LogUserJoined(SocketGuildUser user)
    {
        if (!_active) return;

        var guildId = user.Guild.Id;
        var config = LoggingConfigRepository.Get(guildId);

        if (config == null) return;

        if (config.LogUserJoined)
        {
            var title = GetResource(guildId, "user_joined_title");
            var text = GetResource(guildId, "user_joined_text");
            var ageString = OffsetToDate(DateTime.Now - user.CreatedAt, guildId);

            text = string.Format(text, user.GetDiscriminatedUser(), user.Id, ageString);

            await LogToDefaultChannel(" ", EmbedGenerator.GenerateLoggingEmbed(title, text), guildId);
        }
    }
}