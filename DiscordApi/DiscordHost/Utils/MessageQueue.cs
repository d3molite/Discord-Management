using Discord.WebSocket;
using System.Text;
using System.Timers;
using Serilog;

namespace DiscordApi.DiscordHost.Utils
{
    public class MessageQueue
    {
        private readonly List<SocketMessage> _messageQueue;
        private readonly int _maxMessages;
        private static readonly int _checkInterval = 1;

        private readonly int _intervalMS = _checkInterval * 1000 * 60;
        private readonly int _deleteDuration = 5;

        private System.Timers.Timer _timer;
        public List<SocketMessage> Queue => _messageQueue;

        public MessageQueue(int maxMessages)
        {
            _messageQueue = new();
            _maxMessages = maxMessages;
            _timer = new System.Timers.Timer(_intervalMS);
            _timer.Elapsed += CheckQueue;
            _timer.Start();
        }

        public void Enqueue(SocketMessage message)
        {
            if (_messageQueue.Count == _maxMessages)
            {
                _messageQueue.RemoveAt(0);
            }

            _messageQueue.Add(message);
        }

        public void Clear()
        {
            _messageQueue.Clear();
        }

        public Dictionary<SocketTextChannel, List<SocketMessage>> GetGroupedByChannels()
        {
            Dictionary<SocketTextChannel, List<SocketMessage>> dict = _messageQueue
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

            foreach (SocketMessage message in Queue)
            {
                sb.AppendLine($"{message.Author} at {message.Timestamp}: {message.Content}");
            }

            return sb.ToString();
        }

        public static string MessageList(List<SocketMessage> messages)
        {
            var sb = new StringBuilder();

            foreach (var message in messages)
            {
                sb.AppendLine(message.Content);
            }

            return sb.ToString();
        }

        private void CheckQueue(Object source, ElapsedEventArgs e)
        {
            try
            {
                foreach (var message in Queue.ToList())
                {
                    if ((DateTime.Now - message.Timestamp).TotalMinutes > _deleteDuration)
                    {
                        Queue.Remove(message);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
