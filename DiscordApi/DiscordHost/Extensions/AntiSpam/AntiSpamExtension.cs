using Discord.WebSocket;
using DiscordApi.DiscordHost.Extensions.Base;
using DiscordApi.DiscordHost.Extensions.Interfaces;
using DiscordApi.DiscordHost.Utils;
using DiscordApi.Models;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.AntiSpam;

public class AntiSpamExtension : ClientExtension
{
    private static readonly int _maxQueueLength = 10;

    private readonly ILoggingExtension _logger;

    private readonly GuildMessages _messages = new(_maxQueueLength);

    private readonly List<AntiSpamTimeHandler> _spamTimeHandlers = new();

    public AntiSpamExtension(DiscordSocketClient client, ILoggingExtension logger, string botName) : base(botName,
        client)
    {
        _logger = logger;

        BotName = botName;

        Client.MessageReceived += CheckForSpam;

        Log.Information("AntiSpam attached to {ClientName}", BotName);
    }

    private async Task CheckForSpam(SocketMessage message)
    {
        if (string.IsNullOrEmpty(message.Content) || message.Author.Id == Client.CurrentUser.Id) return;

        var channel = (SocketTextChannel)message.Channel;
        var guild = channel.Guild;
        var author = message.Author;
        var user = (SocketGuildUser)author;

        if (TryGetConfig<AntiSpamConfig>(guild.Id, out var config))
        {
            // see if a spam cleaner for this user in this guild is already runnning
            if (_spamTimeHandlers.Any(x => x.User == user && x.Guild == guild))
            {
                var cleaner = _spamTimeHandlers.First(x => x.User == user);
                RestartAndInsert(cleaner, message);
                return;
            }

            if (config.IgnorePrefixes != null)
                try
                {
                    var pfx = config.IgnorePrefixes.Split(",");
                    if (!pfx.Any(x => message.Content.StartsWith(x))) _messages.InsertIntoQueue(message, guild);
                }
                catch
                {
                    Log.Debug("Could not split prefixes.");
                }

            else _messages.InsertIntoQueue(message, guild);

            var queue = _messages.GetQueue(message, guild);

            if (CheckQueueForSpam(queue))
            {
                Log.Debug("Spam Detected by {BotName}", BotName);

                if (_spamTimeHandlers.Any(x => x.User == user))
                {
                    _spamTimeHandlers.First(x => x.User == user).Restart();
                    Log.Debug("Restarted Handler for {User}", user.Nickname);
                }
                else
                {
                    var handler = new AntiSpamTimeHandler(_logger, BotName, Client)
                    {
                        Config = config,
                        Guild = guild,
                        User = user,
                        Queue = queue
                    };

                    handler.SpamDeleted += RemoveHandler;

                    _spamTimeHandlers.Add(handler);
                }
            }
        }
    }

    private void RestartAndInsert(AntiSpamTimeHandler cleaner, SocketMessage message)
    {
        cleaner.Queue.ForceEnqueue(message);
        cleaner.Restart();
        Log.Verbose("Restarted Handler for {User}", message.Author.Username);
    }

    private void RemoveHandler(object? sender, EventArgs e)
    {
        var h = _spamTimeHandlers.FirstOrDefault(x => x.Finished);
        if (h != null) _spamTimeHandlers.Remove(h);
    }

    private bool CheckQueueForSpam(MessageQueue queue)
    {
        var spams = queue.Queue
            .GroupBy(message => message.Content)
            .Where(group => group.Count() > 5);

        if (spams.Any()) return true;

        return false;
    }
}