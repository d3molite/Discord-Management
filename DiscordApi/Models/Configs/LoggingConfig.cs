namespace DiscordApi.Models
{
    public class LoggingConfig : IConfig<LoggingConfig>
    {
        public int ID { get; set; }
        public bool EnableLogging { get; set; }

        public bool LogMessageDeleted { get; set; }

        public bool LogUserJoined { get; set; }

        public bool LogUserLeft { get; set; }
        public bool LogUserKicked { get; set; }
        public bool LogUserBanned { get; set; }

        public ulong LoggingChannelID { get; set; }
    }
}
