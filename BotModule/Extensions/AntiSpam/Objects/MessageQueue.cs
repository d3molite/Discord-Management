using System.Text;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace BotModule.Extensions.AntiSpam.Objects;

public class MessageQueue
{
    private const int TimeUntilDeletion = 5;
    private const int CheckInterval = 1;
    private readonly int _maxMessages;

    private readonly PeriodicTimer _timer;

    public readonly CancellationTokenSource TokenSource = new();

    public MessageQueue(int maxMessages)
    {
        _maxMessages = maxMessages;
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(CheckInterval));
        CheckQueueEvent += CheckQueue;

        Task.Run(async () => await TimerTask());
    }

    public List<IMessage> Queue { get; } = new();

    public bool IsEmpty => Queue.Any();

    public event EventHandler? CheckQueueEvent;

    /// <summary>
    ///     Enqueue a message into the queue.
    ///     After <see cref="_maxMessages" /> has been reached, the oldest message will be deleted.
    /// </summary>
    /// <param name="message">Message to enqueue</param>
    public void Enqueue(IMessage message)
    {
        if (Queue.Count >= _maxMessages) Queue.RemoveAt(0);
        Queue.Add(message);
    }

    /// <summary>
    ///     Forces a message into the queue, regardless of the queue size.
    /// </summary>
    /// <param name="message">Message to enqueue</param>
    public void ForceEnqueue(IMessage message)
    {
        if (!Queue.Contains(message))
            Queue.Add(message);
    }

    /// <summary>
    ///     Clears the message queue.
    /// </summary>
    public void Clear()
    {
        Queue.Clear();
    }

    /// <summary>
    ///     Task which checks the queue every minute to remove messages older than 5 minutes.
    /// </summary>
    private async Task TimerTask()
    {
        while (await _timer.WaitForNextTickAsync(TokenSource.Token)) CheckQueueEvent?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    ///     Triggers a queue check externally.
    /// </summary>
    public void TriggerExternalCheck()
    {
        CheckQueueEvent?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    ///     Method which checks the queue.
    /// </summary>
    private void CheckQueue(object? e, EventArgs ee)
    {
        try
        {
            var oldMessages = Queue.ToList().Where(message =>
                (DateTime.Now - message.Timestamp).TotalMinutes > TimeUntilDeletion);

            foreach (var message in oldMessages) Queue.Remove(message);
        }
        catch (InvalidOperationException ex)
        {
            Log.Error("Error while removing from queue: {Exception}", ex.Message);
        }
    }

    /// <summary>
    ///     Groups all queued messages by their channel
    /// </summary>
    /// <returns>A dictionary of grouped messages.</returns>
    public Dictionary<SocketTextChannel, List<IMessage>> GetGroupedByChannels()
    {
        return Queue
            .GroupBy(
                x => (SocketTextChannel)x.Channel
            )
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );
    }

    /// <summary>
    ///     Lists all messages in the queue.
    /// </summary>
    /// <returns>A list of all queued messages</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var message in Queue) sb.AppendLine($"{message.Author} at {message.Timestamp}: {message.Content}");

        return sb.ToString();
    }
}