using BotModule.Extensions.ImageManipulation.Core;
using ImageMagick;

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

        ResizeIfTooLarge(image);

        ResizeFurtherIfTooLarge(image);
    }

    private static void ResizeIfTooLarge(Image image)
    {
        var fileInfo = new FileInfo(image.TargetPath);

        if (fileInfo.Length <= 8000000) return;

        using var optimizable = new MagickImageCollection(fileInfo);

        var zeroFrame = optimizable[0];

        foreach (var t in optimizable)
        {
            t.Map(zeroFrame);
            t.ColorFuzz = (Percentage)20;
        }

        var settings = new QuantizeSettings
        {
            Colors = 64,
            DitherMethod = DitherMethod.FloydSteinberg
        };

        optimizable.Quantize(settings);

        optimizable.Write(image.TargetPath);

        optimizable.Dispose();
    }

    private static void ResizeFurtherIfTooLarge(Image image)
    {
        var fileInfo = new FileInfo(image.TargetPath);

        if (fileInfo.Length <= 8000000) return;

        using var newOptimizer = new MagickImageCollection(fileInfo);

        using var output = new MagickImageCollection();

        for (var index = 0; index < newOptimizer.Count; index++)
        {
            var t = newOptimizer[index];

            if (index % 2 != 0) continue;

            t.AnimationDelay = image.Delay * 2;
            output.Add(t);
        }

        output.Write(image.TargetPath);

        newOptimizer.Dispose();
    }
}