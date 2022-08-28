using System.Globalization;
using System.Resources;
using System.Timers;
using Discord;
using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.DiscordHost.Extensions.Interfaces;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Models;
using DiscordApi.Resources.Extensions;
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

        Resources = new ResourceManager(typeof(AntiSpamResources));

        Finished = false;
    }

    public AntiSpamConfig Config { get; set; }
    public SocketGuild Guild { get; set; }
    public MessageQueue Queue { get; set; }
    public SocketGuildUser User { get; set; }

    public bool Finished { get; set; }

    public event EventHandler SpamDeleted;

    private async void DeleteSpam(object? sender, ElapsedEventArgs e)
    {
        _timer.Stop();
        await DeleteAsync();
    }

    private async Task DeleteAsync()
    {
        var culture = new CultureInfo(LocalizationService.GetLocale(BotName, Guild.Id));

        _logger.Pause();

        foreach (var kvp in Queue.GetGroupedByChannels()) await kvp.Key.DeleteMessagesAsync(kvp.Value);

        Thread.Sleep(100);

        _logger.Resume();

        Role role = null;

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


        if (TryGetConfig<LoggingConfig>(Guild.Id, out var c))
        {
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

        Queue.Clear();

        Finished = true;

        SpamDeleted.Invoke(null, null);
    }

    private Embed DeletedMessageOverview(MessageQueue queue)
    {
        var deleted = queue.GetGroupedByChannels();

        var culture = new CultureInfo(LocalizationService.GetLocale(BotName, Guild.Id));

        var embed = new EmbedBuilder
        {
            Title = string.Format(
                GetResource(nameof(AntiSpamResources.deleted_spam_title), culture)!,
                GetDiscriminatedUser(User))
        };

        foreach (var kvp in deleted) embed.AddField(kvp.Key.Name + ":", MessageQueue.MessageList(kvp.Value));

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