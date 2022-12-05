using Discord.WebSocket;

namespace BotModule.Extensions.AntiSpam.Objects;

public class UserMessageCache
{
	private readonly Dictionary<SocketUser, SocketMessageQueue> _userMessages = new();
	private readonly int _maxMessages;

	public UserMessageCache(int maxMessages)
	{
		_maxMessages = maxMessages;
		Task.Run(async () => await DisposeEmpty());
	}

	private async Task DisposeEmpty()
	{
		var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));

		while (await timer.WaitForNextTickAsync(CancellationToken.None))
		{
			var emptyQueueKeys = _userMessages.Where(x => x.Value.IsEmpty).Select(x => x.Key).ToList();

			foreach (var key in emptyQueueKeys)
			{
				_userMessages[key].TokenSource.Cancel();
				_userMessages.Remove(key);
			}
		}
	}

	public SocketMessageQueue GetOrCreate(SocketUser messageAuthor)
	{
		if (_userMessages.TryGetValue(messageAuthor, out var messageQueue)) return messageQueue;

		var newMessageQueue = new SocketMessageQueue(_maxMessages);
		_userMessages.Add(messageAuthor, newMessageQueue);
		return newMessageQueue;
	}
}