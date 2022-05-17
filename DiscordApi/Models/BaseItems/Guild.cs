namespace DiscordApi.Models
{
    public class Guild
    {
        /// <summary>
        /// Gets or sets the Database ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Server Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ServerID.
        /// </summary>
        public ulong GuildID { get; set; }
    }
}
