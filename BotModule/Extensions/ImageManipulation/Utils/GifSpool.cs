using BotModule.Extensions.ImageManipulation.Core;
using ImageMagick;
using IMG = System.Drawing.Image;

namespace BotModule.Extensions.ImageManipulation.Utils;

public class GifSpool
{
    public static void Unroll(Image image)
    {
        if (!Directory.Exists(image.FrameFolder)) Directory.CreateDirectory(image.FrameFolder);

        var fileInfo = new FileInfo(image.SourcePath);

        using var collection = new MagickImageCollection(fileInfo);

        var length = collection.Count;

        image.Frames = length;

        var delay = (int)collection.Average(x => x.AnimationDelay);
        image.Delay = delay;

        collection.Coalesce();

        for (var i = 0; i < length; i++)
        {
            var frame = collection[i];
            frame.Write(image.FramePath(i));
        }
    }

    public static void Roll(Image image, bool manipulated)
    {
        using var collection = new MagickImageCollection();

        for (var i = 0; i < image.Frames; i++)
        {
            collection.Add(manipulated ? image.ModifiedFramePath(i) : image.FramePath(i));
            collection[i].AnimationDelay = image.Delay;
        }

        collection.Optimize();

        if (manipulated)
            collection.Write(image.TargetPath);
    }
}