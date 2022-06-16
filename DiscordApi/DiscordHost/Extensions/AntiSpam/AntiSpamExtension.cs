using Discord;
using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Interfaces;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Models;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.AntiSpam;

public class AntiSpamExtension : Extension
{
    private static readonly int _maxQueueLength = 10;

    private readonly DiscordSocketClient _client;
    private readonly ILoggingExtension _logger;

    private readonly GuildMessages _messages = new(_maxQueueLength);

    public AntiSpamExtension(DiscordSocketClient client, ILoggingExtension logger, string botName) : base(botName)
    {
        _client = client;
        _logger = logger;

        BotName = botName;

        _client.MessageReceived += CheckForSpam;

        Log.Information("AntiSpam attached to {ClientName}", BotName);
    }

    public string BotName { get; set; }

    private async Task CheckForSpam(SocketMessage message)
    {
        if (string.IsNullOrEmpty(message.Content) || message.Author == _client.CurrentUser) return;

        var channel = (SocketTextChannel)message.Channel;
        var guild = channel.Guild;
        var author = message.Author;
        var user = (SocketGuildUser)author;

        if (TryGetConfig<AntiSpamConfig>(guild.Id, out var config))
        {

            if (config.IgnorePrefixes != null)
            {
                try
                {
                    var pfx = config.IgnorePrefixes.Split(",");
                    if (!pfx.Any(x => message.Content.StartsWith(x)))
                    {
                        _messages.InsertIntoQueue(message, guild);
                    }
                }
                catch
                {
                    Log.Debug("Could not split prefixes.");
                }
                
            }
            else _messages.InsertIntoQueue(message, guild);
            
            var queue = _messages.GetQueue(message, guild);

            if (CheckQueueForSpam(queue))
            {
                _logger.Pause();

                foreach (var kvp in queue.GetGroupedByChannels()) await kvp.Key.DeleteMessagesAsync(kvp.Value);

                var role = config.MutedRole;
                if (role != null) await Mute(user, guild.GetRole(role.RoleID));
                else await user.SetTimeOutAsync(TimeSpan.FromDays(3), new RequestOptions(){AuditLogReason = $"Spam AutoDetected by {BotName}"});

                if (TryGetConfig<LoggingConfig>(guild.Id, out var c))
                {
                    if (role != null)
                    {
                        await _logger.SendCustomLogMessage(
                            " ",
                            _logger.GenerateEmbed(
                                "User Muted for Spamming",
                                "Muted User <@" + author.Id + "> for spamming.", Color.DarkRed
                            ), c.LoggingChannelID);
                    }
                    else
                    {
                        await _logger.SendCustomLogMessage(
                            " ",
                            _logger.GenerateEmbed(
                                "User Timed Out for Spamming",
                                "Timed Out User <@" + author.Id + "> for spamming (3 Days).", Color.DarkRed
                            ), c.LoggingChannelID);
                    }

                    await _logger.SendCustomLogMessage(
                        " ",
                        DeletedMessageOverview(queue),
                        c.LoggingChannelID);
                }

                queue.Clear();

                await Task.Delay(1000);

                _logger.Resume();
            }
        }
    }

    private async Task Mute(SocketGuildUser user, SocketRole muted)
    {
        var roles = user.Roles.Where(x => x.Name != "everyone").ToArray();
        await user.RemoveRolesAsync(roles);
        await user.AddRoleAsync(muted);
    }

    private bool CheckQueueForSpam(MessageQueue queue)
    {
        var spams = queue.Queue
            .GroupBy(message => message.Content)
            .Where(group => group.Count() > 5);

        if (spams.Any()) return true;

        return false;
    }

    private Embed DeletedMessageOverview(MessageQueue queue)
    {
        var deleted = queue.GetGroupedByChannels();

        var embed = new EmbedBuilder
        {
            Title = "Bulk Deleted Spam by " + deleted.First().Value.First().Author.Username + ":"
        };

        foreach (var kvp in deleted) embed.AddField(kvp.Key.Name + ":", MessageQueue.MessageList(kvp.Value));

        return embed.Build();
    }
}