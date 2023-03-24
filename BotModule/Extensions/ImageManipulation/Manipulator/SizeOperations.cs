using OpenCvSharp;
using Serilog;

namespace BotModule.Extensions.ImageManipulation.Manipulator;

public static partial class ImageManipulator
{
    private static void MakeSquare(Mat input)
    {
        if (input.Width == input.Height) return;

        if (input.Height > input.Width)
            Cv2.Resize(input, input, new Size(input.Width, input.Width));

        else if (input.Width > input.Height)
            Cv2.Resize(input, input, new Size(input.Height, input.Height));
    }

    private static Mat MakeSmallerSquare(Mat mat)
    {
        const double width = 500.0;

        try
        {
            if (!(mat.Width > width)) return mat;

            var scaleFactor = width / mat.Width;
            var height = mat.Height * scaleFactor;

            Cv2.Resize(mat, mat, new Size(width, height));

            return mat;
        }
        catch (Exception ex)
        {
            Log.Error("Exception in Resizer {exception}", ex);
        }

        return new Mat(mat);
    }
}