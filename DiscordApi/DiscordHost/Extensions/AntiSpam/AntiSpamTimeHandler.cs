using System.Globalization;
using System.Timers;
using Discord;
using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.DiscordHost.Extensions.Interfaces;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Models;
using DiscordApi.Services;
using Serilog;
using Timer = System.Timers.Timer;

namespace DiscordApi.DiscordHost.Extensions.AntiSpam;

public class AntiSpamTimeHandler : ClientExtension
{
    private readonly ILoggingExtension _logger;
    private readonly Timer _timer;

    public AntiSpamTimeHandler(
        ILoggingExtension logger,
        string botName, DiscordSocketClient client)
        : base(botName, client)
    {
        _logger = logger;

        _timer = new Timer(2000);
        _timer.Elapsed += DeleteSpam;

        _timer.Start();

        Finished = false;
    }

    public AntiSpamConfig Config { get; set; }
    public SocketGuild Guild { get; set; }
    public MessageQueue Queue { get; set; }
    public SocketGuildUser User { get; set; }

    private string MessageContent { get; set; }
    private Dictionary<string, List<string>> Deleted { get; } = new();

    public bool Finished { get; set; }

    public event EventHandler SpamDeleted;

    private async void DeleteSpam(object? sender, ElapsedEventArgs e)
    {
        _timer.Stop();
        await DeleteAsync();
    }

    private async Task SendLog()
    {
        if (TryGetConfig<LoggingConfig>(Guild.Id, out var c))
        {
            var role = Config.MutedRole;

            var culture = new CultureInfo(LocalizationService.GetLocale(BotName, Guild.Id));

            if (role != null)
            {
                await _logger.SendCustomLogMessage(
                    " ",
                    _logger.GenerateEmbed(
                        GetResource("user_muted_title", culture)!,
                        string.Format(
                            GetResource("user_muted_text", culture)!,
                            GetDiscriminatedUser(User),
                            User.Id.ToString()),
                        Color.DarkRed
                    ), c.LoggingChannelID);
            }
            else
            {
                await _logger.SendCustomLogMessage(
                    " ",
                    _logger.GenerateEmbed(
                        GetResource("user_timedout_title", culture)!,
                        string.Format(
                            GetResource("user_timedout_text", culture)!,
                            GetDiscriminatedUser(User),
                            User.Id),
                        Color.DarkRed
                    ), c.LoggingChannelID);
            }

            await _logger.SendCustomLogMessage(
                " ",
                DeletedMessageOverview(Queue),
                c.LoggingChannelID);
        }
    }

    private async Task MuteUser()
    {
        Role? role = null;

        try
        {
            role = Config.MutedRole;

            if (role != null) await Mute(User, Guild.GetRole(role.RoleID));

            else
                await User.SetTimeOutAsync(TimeSpan.FromDays(3),
                    new RequestOptions { AuditLogReason = $"Spam AutoDetected by {BotName}" });
        }
        catch
        {
            Log.Error("Could not apply mute to {User}", User);
        }
    }

    private async Task DeleteAsync()
    {
        await MuteUser();

        _logger.Pause();

        foreach (var kvp in Queue.GetGroupedByChannels())
        {
            MessageContent = kvp.Value.First().Content;
            await kvp.Key.DeleteMessagesAsync(kvp.Value);

            if (Deleted.ContainsKey(kvp.Key.Name))
            {
                Deleted[kvp.Key.Name].AddRange(kvp.Value.Select(x => x.Content).ToList());
            }
            else
            {
                Deleted.Add(kvp.Key.Name, kvp.Value.Select(x => x.Content).ToList());
            }
        }

        await CleanUp();

        Thread.Sleep(100);

        _logger.Resume();

        await SendLog();

        Queue.Clear();

        Finished = true;
        SpamDeleted.Invoke(null, null);
    }

    private async Task CleanUp()
    {
        foreach (var channel in Guild.Channels)
        {
            if (channel is not ISocketMessageChannel messageChannel) continue;

            try
            {
                var messages =
                    await messageChannel
                        .GetMessagesAsync(5)
                        .Select(
                            x =>
                                x.First(y => y.Content == MessageContent && y.Author.Id == User.Id))
                        .ToListAsync();


                foreach (var message in messages)
                {
                    await message.DeleteAsync();

                    if (Deleted.ContainsKey(channel.Name))
                    {
                        Deleted[channel.Name].Add(message.Content);
                    }
                    else
                    {
                        Deleted.Add(channel.Name, new List<string> { message.Content });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Could not delete. {ex}", ex);
            }
        }
    }

    private Embed DeletedMessageOverview(MessageQueue queue)
    {
        var culture = new CultureInfo(LocalizationService.GetLocale(BotName, Guild.Id));

        var embed = new EmbedBuilder
        {
            Title = string.Format(
                GetResource("nameof(AntiSpamResources.deleted_spam_title)", culture)!,
                GetDiscriminatedUser(User))
        };

        foreach (var kvp in Deleted) embed.AddField(kvp.Key + ":", string.Join(Environment.NewLine, kvp.Value));

        return embed.Build();
    }

    private async Task Mute(SocketGuildUser user, SocketRole muted)
    {
        var roles = user.Roles.Where(x => x.Name != "everyone").ToArray();
        await user.RemoveRolesAsync(roles);
        await user.AddRoleAsync(muted);
    }

    public void Restart()
    {
        _timer.Stop();
        _timer.Start();
    }
}