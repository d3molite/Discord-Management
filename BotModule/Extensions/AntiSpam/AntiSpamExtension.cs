using BotModule.DI;
using BotModule.Extensions.AntiSpam.Objects;
using BotModule.Extensions.Base;
using BotModule.Extensions.Logging;
using DB.Repositories;
using Discord.WebSocket;
using Serilog;

namespace BotModule.Extensions.AntiSpam;

public sealed class AntiSpamExtension : ClientExtension
{
    private const int QueueLength = 10;
    private readonly ILanguageProvider _languageProvider;
    private readonly ILoggingProvider _logger;
    private readonly GuildMessageCache _messages = new(QueueLength);
    private readonly List<SpamCleaner> _spamCleaners = new();

    public AntiSpamExtension(DiscordSocketClient client, string botName, ILoggingProvider logger,
        ILanguageProvider provider) : base(client,
        botName, provider)
    {
        Client.MessageReceived += CheckForSpam;
        _logger = logger;
        _languageProvider = provider;
    }

    /// <summary>
    ///     Check if the message is spam.
    /// </summary>
    /// <param name="message">Received Message</param>
    /// <returns></returns>
    private async Task CheckForSpam(SocketMessage message)
    {
        if (string.IsNullOrEmpty(message.Content) || message.Author.Id == Client.CurrentUser.Id) return;

        var channel = (SocketTextChannel)message.Channel;
        var guild = channel.Guild;
        var author = message.Author;
        var user = (SocketGuildUser)author;

        var config = AntiSpamConfigRepository.Get(guild.Id);

        if (config == null)
        {
            Log.Debug("Could not find an AntiSpam Config for {ClientName} on {GuildName}", BotName, guild.Name);
            return;
        }

        // see if a spam cleaner for this user in this guild is already runnning
        if (_spamCleaners.Any(x => x.User == user && x.Guild == guild))
        {
            var cleaner = _spamCleaners.First(x => x.User == user);
            RestartAndInsert(cleaner, message);
            return;
        }

        if (config.IgnorePrefixes != null)
            try
            {
                var prefixes = config.IgnorePrefixes.Split(",").Select(x => x.Trim());

                if (!prefixes.Any(x => message.Content.StartsWith(x)))
                    _messages.Insert(message, guild);
            }
            catch
            {
                Log.Error("Could not split prefixes in AntiSpamConfig with ID {ConfigId}", config.Id);
            }
        else
            _messages.Insert(message, guild);

        var queue = _messages.GetQueue(message, guild);

        // If Spam has been detected
        if (ContainsSpam(queue))
        {
            Log.Debug("Detected Spam from {UserName} in {GuildName}",
                user.GetDiscriminatedUser(),
                guild.Name);

            if (_spamCleaners.Any(x => x.User == user)) return;

            var logger = _logger.Retrieve(Client, guild);
            var handler = new SpamCleaner(logger, _languageProvider)
            {
                User = user,
                Guild = guild,
                Config = config,
                Queue = queue,
                Client = Client
            };

            handler.SpamDeleted += Cleanup;
            _spamCleaners.Add(handler);
        }
    }

    private void RestartAndInsert(SpamCleaner cleaner, SocketMessage message)
    {
        cleaner.Queue.ForceEnqueue(message);
        cleaner.Restart();
        Log.Verbose("Restarted Handler for {User}", message.Author.GetDiscriminatedUser());
    }

    /// <summary>
    ///     Method to trigger after spam has been removed.
    /// </summary>
    private void Cleanup(object? sender, EventArgs e)
    {
        var h = _spamCleaners.FirstOrDefault(x => x.Finished);
        if (h != null) _spamCleaners.Remove(h);
    }

    /// <summary>
    ///     Checks if a given message queue contains spam.
    /// </summary>
    /// <param name="queue"></param>
    /// <returns>True if spam was found, else false</returns>
    private static bool ContainsSpam(MessageQueue queue)
    {
        return queue.Queue.GroupBy(message => message.Content).Any(group => group.Count() > 5);
    }
}