using System.Timers;
using BotModule.DI;
using BotModule.Extensions.Logging;
using DB.Models.Configs.Extensions;
using Discord;
using Discord.WebSocket;
using Serilog;
using Strings.Manager;
using Timer = System.Timers.Timer;

namespace BotModule.Extensions.AntiSpam.Objects;

public class SpamCleaner
{
    private readonly DiscordSocketClient _client;
    private readonly Dictionary<string, List<string>> _deleted = new();
    private readonly ILanguageProvider _languageProvider;
    private readonly ILoggingExtension? _logger;
    private readonly Timer _timer;
    private bool _locked;

    public SpamCleaner(ILoggingExtension? logger, ILanguageProvider languageProvider)
    {
        _logger = logger;
        _languageProvider = languageProvider;

        _timer = new Timer(2000);
        _timer.Elapsed += DeleteSpam;

        _timer.Start();
    }

    public SocketGuildUser User { get; set; }

    public SocketGuild Guild { get; set; }

    public AntiSpamConfig Config { get; set; }

    public MessageQueue Queue { get; set; }

    public DiscordSocketClient Client { get; set; }

    public bool Finished { get; internal set; }

    public event EventHandler SpamDeleted;

    public void Restart()
    {
        if (_locked) return;
        _timer.Stop();
        _timer.Start();
    }

    /// <summary>
    ///     Deletes the Spam of the current user after the timer elapses.
    /// </summary>
    private async void DeleteSpam(object? sender, ElapsedEventArgs e)
    {
        _locked = true;
        _timer.Stop();

        await MuteOrTimeout();

        _logger?.Pause();

        foreach (var (channel, messages) in Queue.GetGroupedByChannels())
            try
            {
                await channel.DeleteMessagesAsync(messages);

                var messageList = messages.Select(x => x.Content).ToList();

                if (_deleted.TryGetValue(channel.Name, out var value))
                    value.AddRange(messageList);
                else
                    _deleted.Add(channel.Name, messageList);
            }
            catch (Exception ex)
            {
                Log.Error("Could not delete in channel {Channel}: {ex}", channel, ex);
            }

        Thread.Sleep(100);

        _logger?.Resume();

        await SendLog();

        Queue.Clear();

        Finished = true;

        SpamDeleted?.Invoke(null, EventArgs.Empty);
    }

    private async Task SendLog()
    {
        if (_logger != null)
        {
            if (Config.MutedRole != null)
            {
                var embed = EmbedGenerator.GenerateLoggingEmbed(
                    GetResource("user_muted_title"),
                    string.Format(GetResource("user_muted_text"), User.GetDiscriminatedUser(), User.Id),
                    Color.DarkRed);

                await _logger.LogToDefaultChannel(" ", embed, Guild.Id);
            }
            else
            {
                var embed = EmbedGenerator.GenerateLoggingEmbed(
                    GetResource("user_timedout_title"),
                    string.Format(GetResource("user_timedout_text"), User.GetDiscriminatedUser(), User.Id),
                    Color.DarkRed);

                await _logger.LogToDefaultChannel(" ", embed, Guild.Id);
            }

            await _logger.LogToDefaultChannel(" ", DeletedMessageOverview(), Guild.Id);
        }
    }

    private string GetResource(string resourceKey)
    {
        return _languageProvider.GetResource(Guild.Id, Client.CurrentUser.Id, ResourceLookup.ResourceGroup.AntiSpam,
            resourceKey);
    }

    private Embed DeletedMessageOverview()
    {
        var embed = new EmbedBuilder
        {
            Title = string.Format(
                GetResource("deleted_spam_title"),
                User.GetDiscriminatedUser())
        };

        foreach (var kvp in _deleted)
            embed.AddField(kvp.Key + ":", string.Join(Environment.NewLine, kvp.Value));

        return embed.Build();
    }

    /// <summary>
    ///     Mutes or Times out the user based on the config contents.
    /// </summary>
    private async Task MuteOrTimeout()
    {
        try
        {
            var role = Config.MutedRole;

            if (role != null)
            {
                await Mute(Guild.GetRole(role.Snowflake));
                return;
            }

            await Timeout();
        }
        catch (Exception ex)
        {
            Log.Error("Could not apply muted role to {User}", User);
            Log.Error("Exception: {ex}", ex);
            await Timeout();
        }
    }

    private async Task Timeout()
    {
        try
        {
            await User.SetTimeOutAsync(TimeSpan.FromDays(3), new RequestOptions
            {
                AuditLogReason = "Spam detected by {BotName}"
            });
        }
        catch (Exception ex)
        {
            Log.Error("Could not time out user {User}: {ex}", User, ex);
        }
    }

    private async Task Mute(SocketRole muted)
    {
        try
        {
            var roles = User.Roles.Where(x => x.Name != "everyone").ToArray();
            await User.RemoveRolesAsync(roles);
            await User.AddRoleAsync(muted);
        }
        catch (Exception ex)
        {
            Log.Error("Could not apply muted role to user {User}: {ex}", User, ex);
        }
    }
}