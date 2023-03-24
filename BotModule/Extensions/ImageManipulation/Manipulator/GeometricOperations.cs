using BotModule.Extensions.ImageManipulation.Core;
using OpenCvSharp;

namespace BotModule.Extensions.ImageManipulation.Manipulator;

public static partial class ImageManipulator
{
    public static Task Mirror(string input, string output, MirrorDirection direction)
    {
        using var image = new Mat(input);
        var width = image.Width;
        var height = image.Height;
        var halfWidth = width / 2;
        var halfHeight = height / 2;

        Rect source;
        Rect target;
        FlipMode flipMode;

        switch (direction)
        {
            default:
                source = new Rect(0, 0, halfWidth, height);
                target = new Rect(halfWidth, 0, halfWidth, height);
                flipMode = FlipMode.Y;
                break;

            case MirrorDirection.LTR:
                source = new Rect(0, 0, halfWidth, height);
                target = new Rect(halfWidth, 0, halfWidth, height);
                flipMode = FlipMode.Y;
                break;

            case MirrorDirection.RTL:
                source = new Rect(halfWidth, 0, halfWidth, height);
                target = new Rect(0, 0, halfWidth, height);
                flipMode = FlipMode.Y;
                break;

            case MirrorDirection.TTB:
                source = new Rect(0, 0, width, halfHeight);
                target = new Rect(0, halfHeight, width, halfHeight);
                flipMode = FlipMode.X;
                break;

            case MirrorDirection.BTT:
                source = new Rect(0, halfHeight, width, halfHeight);
                target = new Rect(0, 0, width, halfHeight);
                flipMode = FlipMode.X;
                break;
        }

        var startHalf = image[source];
        var endHalf = image[target];

        var flipped = startHalf.Flip(flipMode);

        flipped.CopyTo(endHalf);

        image.SaveImage(output);

        image.Dispose();

        return Task.CompletedTask;
    }
}