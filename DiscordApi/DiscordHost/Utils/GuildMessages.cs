using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Utils
{
    public class GuildMessages
    {
        private Dictionary<SocketGuild, UserMessages> _guildMessages;
        private readonly int _maxMessages;

        public GuildMessages(int maxMessages)
        {
            _guildMessages = new();
            _maxMessages = maxMessages;
        }


        public void InsertIntoQueue(SocketMessage message, SocketGuild guild)
        {
            var userMessages = GetOrCreateNew(guild);
            var messageQueue = userMessages.GetOrCreateNew(message.Author);
            messageQueue.Enqueue(message);
        }

        public MessageQueue GetQueue(SocketMessage message, SocketGuild guild)
        {
            var userMessages = GetOrCreateNew(guild);
            return userMessages.GetOrCreateNew(message.Author);
        }

        private UserMessages GetOrCreateNew(SocketGuild guild)
        {
            if (_guildMessages.ContainsKey(guild))
            {
                return _guildMessages[guild];
            }

            var userMessages = new UserMessages(_maxMessages);
            _guildMessages.Add(guild, userMessages);

            return userMessages;
        }
    }
}
