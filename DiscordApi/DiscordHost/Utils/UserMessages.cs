using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Utils
{
    public class UserMessages
    {
        private Dictionary<SocketUser, MessageQueue> _userMessages;
        private readonly int _maxMessages;

        public UserMessages(int maxMessages)
        {
            _userMessages = new ();
            _maxMessages = maxMessages;
        }

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
}
