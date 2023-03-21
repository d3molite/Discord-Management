namespace DB.Models.Base;

public class Emoji
{
    public int Id { get; set; }

    public string EmojiString { get; set; } = null!;

    public override string ToString()
    {
        return EmojiString;
    }
}