namespace DiscordApi.Models;

public class Bot
{
    public Bot(int iD, string name, string token, string presence)
    {
        ID = iD;
        Name = name;
        Token = token;
        Presence = presence;
    }

    /// <summary>
    ///     Gets or sets the Database ID.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    ///     Gets or sets the name of the bot.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the Bot Token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    ///     Gets or sets the Presence.
    /// </summary>
    public string Presence { get; set; }

    public bool IsActive { get; set; }

    public bool IsDebug { get; set; }
}