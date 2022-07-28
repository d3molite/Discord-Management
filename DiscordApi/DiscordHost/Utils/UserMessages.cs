using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Utils;

public class UserMessages
{
    private readonly int _maxMessages;
    private readonly Dictionary<SocketUser, MessageQueue> _userMessages;

    public UserMessages(int maxMessages)
    {
        _userMessages = new Dictionary<SocketUser, MessageQueue>();
        _maxMessages = maxMessages;
    }

    public ulong _guildId { get; set; }

    public MessageQueue GetOrCreateNew(SocketUser user)
    {
        if (_userMessages.ContainsKey(user))
        {
            return _userMessages[user];
        }

        var messageQueue = new MessageQueue(_maxMessages);
        _userMessages.Add(user, messageQueue);

        return messageQueue;
    }
}