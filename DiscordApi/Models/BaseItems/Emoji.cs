namespace DiscordApi.Models;

public class Emoji
{
    public int ID { get; set; }

    public string EmojiString { get; set; }

    public override string ToString()
    {
        return EmojiString;
    }
}