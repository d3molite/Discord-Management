using System.Reflection;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public static class MemeResources
{
    private static readonly Random Random = new();

    private static readonly List<string> Paths = new()
    {
        "eyelaugh.png",
        "laugh.png",
        "100.png",
        "felix.png",
        "fire.png",
        "ok_hand.png"
    };

    public static string GetRandom()
    {
        var randomImage = Paths.ElementAt(Random.Next(0, Paths.Count));
        var loc = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent;

        var resourcePath = @"/DiscordHost/Extensions/ImageManipulation/ManipulationHelper/Resources/";

        return Path.GetFullPath(loc + resourcePath + randomImage);
    }
}