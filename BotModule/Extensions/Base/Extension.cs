namespace BotModule.Extensions.Base;

public class Extension
{
    public Extension(string botName)
    {
        BotName = botName;
    }

    public string BotName { get; set; }
}