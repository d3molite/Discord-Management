using BotModule.Extensions.ImageManipulation.Core;
using BotModule.Extensions.ImageManipulation.Utils;
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

    public static Task Mandala(string input, string output, int slice = 1)
    {
        // normalize the starting quadrant value to always be between 1 and 8
        slice = QuadrantHelper.Normalize(slice);

        // set the target slice to the next slice
        var targetSlice = QuadrantHelper.Normalize(slice + 1);

        // read the image
        using var image = new Mat(input);

        // make the image square to work with the math
        MakeSquare(image);

        // initialize the quadrant helper
        var helper = new QuadrantHelper(image.Width, image.Height);

        // get the bounding box of the source triangle
        var boundingRectSource = helper.GetBoundingBox(slice);

        // get the bounding box of the target triangle
        var boundingRectTarget = helper.GetBoundingBox(QuadrantHelper.Normalize(slice + 1));

        // check if the target slice is the first or second
        var first = slice % 2 != 0;

        // get the source image region
        var src = image.Clone(boundingRectSource);

        // get the target image region
        var tar = image[boundingRectTarget];

        // create a black clipping mask in the source rect
        var mask = new Mat(boundingRectTarget.Size, image.Type(), Scalar.Black);
        // inject the source triangle in white
        mask.FillConvexPoly(helper.GetMaskTriangle(targetSlice), Scalar.White);

        // if the slice is the first, get it and paste the target
        if (first)
        {
            var fp = QuadrantHelper.GetFlipMode(slice);

            Cv2.Rotate(src, src, RotateFlags.Rotate90Clockwise);
            src = src.Flip(fp);

            src.CopyTo(tar, mask);
        }

        // if the slice is the second,
        else
        {
            // get the flipmode to paste it into the next quadrant
            var fp = QuadrantHelper.GetFlipMode(targetSlice);

            var fp2 = fp == FlipMode.X ? FlipMode.Y : FlipMode.X;

            src = src.Flip(fp2);
            src.CopyTo(tar, mask);

            Cv2.Rotate(mask, mask, RotateFlags.Rotate90Clockwise);
            Cv2.Rotate(src, src, RotateFlags.Rotate90Clockwise);
            src = src.Flip(fp);
            mask = mask.Flip(fp);

            src.CopyTo(tar, mask);
        }

        List<int> l = new();

        switch (targetSlice)
        {
            default:
                l = new List<int> { 3, 5, 7 };
                break;
            case 3:
            case 4:
                l = new List<int> { 5, 7, 1 };
                break;
            case 5:
            case 6:
                l = new List<int> { 7, 1, 3 };
                break;
            case 7:
            case 8:
                l = new List<int> { 1, 3, 5 };
                break;
        }

        for (var i = 1; i < l.Count + 1; i++)
        {
            var area = image[helper.GetBoundingBox(l[i - 1])];
            tar.CopyTo(area);

            for (var j = i; j > 0; j--) Cv2.Rotate(area, area, RotateFlags.Rotate90Clockwise);
        }

        image.SaveImage(output);

        image.Dispose();

        return Task.CompletedTask;
    }
}