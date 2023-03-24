/*using OpenCvSharp;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public static partial class ImageManipulationHelper
{
   
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
}*/

