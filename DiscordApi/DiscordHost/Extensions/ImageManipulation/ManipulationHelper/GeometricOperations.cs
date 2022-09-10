using OpenCvSharp;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public static partial class ImageManipulationHelper
{
    public static Task Mirror(Image img, MirrorDirection direction)
    {
        using var image = new Mat(img.SourcePath);
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

        image.SaveImage(img.TargetPath);

        image.Dispose();

        return Task.CompletedTask;
    }

    public static Task Mandala(Image img, int slice = 1)
    {
        // normalize the starting quadrant value to always be between 1 and 8
        slice = QuadrantHelper.Normalize(slice);

        // set the target slice to the next slice
        var targetSlice = QuadrantHelper.Normalize(slice + 1);

        // read the image
        using var image = new Mat(img.SourcePath);

        // make the image square to work with the math
        Squarify(image);

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

            for (var j = i; j > 0; j--)
            {
                Cv2.Rotate(area, area, RotateFlags.Rotate90Clockwise);
            }
        }

        image.SaveImage(img.TargetPath);

        image.Dispose();

        return Task.CompletedTask;
    }

    private static void OverlayPng(Mat input, Mat overlay, out Mat result, int x = 0, int y = 0)
    {
        if (overlay.Width > input.Width || overlay.Height > input.Height)
        {
            overlay = overlay.Resize(input.Size());
        }

        if (x + overlay.Width > input.Width)
        {
            x = input.Width - overlay.Width - 5;
        }

        if (y + overlay.Height > input.Height)
        {
            y = input.Height - overlay.Height - 5;
        }

        var mask = new Mat();
        Cv2.Threshold(overlay.Split()[3], mask, 0, 255, ThresholdTypes.Binary);

        Jpegify(overlay, out overlay, 100);

        result = input.Clone();

        try
        {
            var boundingTarget = result[new Rect(x, y, overlay.Width, overlay.Height)];

            overlay.CopyTo(boundingTarget, mask);
        }
        catch
        {
        }
    }

    public static (int, int) GetSurroundingSections(int width, int height)
    {
        var xpositions = width / 4;
        var ypositions = height / 4;

        var quads = new List<(int, int)>
        {
            (0, 0),
            (0, ypositions),
            (0, ypositions * 2),
            (0, ypositions * 3),

            (xpositions, 0),
            (xpositions, ypositions * 3),

            (xpositions * 2, 0),
            (xpositions * 2, ypositions * 3),

            (xpositions * 3, 0),
            (xpositions * 3, ypositions),
            (xpositions * 3, ypositions * 2),
            (xpositions * 2, ypositions * 3)
        };

        var random = Random.Next(0, quads.Count);
        return quads.ElementAt(random);
    }
}