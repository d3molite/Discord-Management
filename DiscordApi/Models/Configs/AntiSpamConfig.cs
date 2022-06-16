namespace DiscordApi.Models
{
    public class AntiSpamConfig : IConfig<AntiSpamConfig>
    {
        public int ID { get; set; }

        public int MessageLimit { get; set; }

        public int LinkLimit { get; set; }

        public int ResetTime { get; set; }
        
        public string? IgnorePrefixes { get; set; }

        public Role? MutedRole { get; set; }
    }
}
