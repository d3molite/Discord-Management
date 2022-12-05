using System.Text;
using Discord.WebSocket;
using Serilog;

namespace BotModule.Extensions.AntiSpam.Objects;

public class SocketMessageQueue
{
	private const int TimeUntilDeletion = 5;
	private const int CheckInterval = 1;
	private readonly int _maxMessages;

	private readonly PeriodicTimer _timer;

	public readonly CancellationTokenSource TokenSource = new();

	public SocketMessageQueue(int maxMessages)
	{
		_maxMessages = maxMessages;
		_timer = new PeriodicTimer(TimeSpan.FromMinutes(CheckInterval));

		Task.Run(async () => await CheckQueue());
	}

	public List<SocketMessage> Queue { get; } = new();

	public bool IsEmpty => Queue.Any();

	/// <summary>
	///     Enqueue a message into the queue.
	///     After <see cref="_maxMessages" /> has been reached, the oldest message will be deleted.
	/// </summary>
	/// <param name="message">Message to enqueue</param>
	public void Enqueue(SocketMessage message)
	{
		if (Queue.Count >= _maxMessages) Queue.RemoveAt(0);
		Queue.Add(message);
	}

	/// <summary>
	///     Forces a message into the queue, regardless of the queue size.
	/// </summary>
	/// <param name="message">Message to enqueue</param>
	public void ForceEnqueue(SocketMessage message)
	{
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
	private async Task CheckQueue()
	{
		while (await _timer.WaitForNextTickAsync(TokenSource.Token))
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