using System.Text;
using System.Timers;
using Discord.WebSocket;
using Serilog;
using Timer = System.Timers.Timer;

namespace DiscordApi.DiscordHost.Utils;

public class MessageQueue
{
    private static readonly int _checkInterval = 1;
    private readonly int _deleteDuration = 5;

    private readonly int _intervalMS = _checkInterval * 1000 * 60;
    private readonly int _maxMessages;

    private readonly Timer _timer;

    public MessageQueue(int maxMessages)
    {
        Queue = new List<SocketMessage>();
        _maxMessages = maxMessages;
        _timer = new Timer(_intervalMS);
        _timer.Elapsed += CheckQueue;
        _timer.Start();
    }

    public List<SocketMessage> Queue { get; }

    public void Enqueue(SocketMessage message)
    {
        if (Queue.Count == _maxMessages) Queue.RemoveAt(0);

        Queue.Add(message);
    }

    public void ForceEnqueue(SocketMessage message)
    {
        if (!Queue.Contains(message))
            Queue.Add(message);
    }

    public void Clear()
    {
        Queue.Clear();
    }

    public Dictionary<SocketTextChannel, List<SocketMessage>> GetGroupedByChannels()
    {
        var dict = Queue
            .GroupBy(
                x => (SocketTextChannel)x.Channel
            )
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

        return dict;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var message in Queue) sb.AppendLine($"{message.Author} at {message.Timestamp}: {message.Content}");

        return sb.ToString();
    }

    public static string MessageList(List<SocketMessage> messages)
    {
        var sb = new StringBuilder();

        foreach (var message in messages) sb.AppendLine(message.Content);

        return sb.ToString();
    }

    private void CheckQueue(object source, ElapsedEventArgs e)
    {
        try
        {
            foreach (var message in Queue.ToList())
                if ((DateTime.Now - message.Timestamp).TotalMinutes > _deleteDuration)
                    Queue.Remove(message);
        }
        catch (InvalidOperationException ex)
        {
            Log.Error(ex.Message);
        }
    }
}