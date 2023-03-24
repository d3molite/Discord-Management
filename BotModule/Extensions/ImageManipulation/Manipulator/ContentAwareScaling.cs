using BotModule.Extensions.ImageManipulation.Utils;
using OpenCvSharp;

namespace BotModule.Extensions.ImageManipulation.Manipulator;

public static partial class ImageManipulator
{
    public static Task ContentAwareScale(string input, string output, int percentage = 20, bool flipOrientation = false)
    {
        using var inputImage = new Mat(input);

        var image = MakeSmallerSquare(inputImage);
        MakeJpeg(image, out image, 75);

        var computed = image.Clone();

        if (percentage > 69) percentage = 69;

        var gray = computed.Clone();
        Cv2.CvtColor(computed, gray, ColorConversionCodes.BGR2GRAY);

        var sobelX = gray.Clone();
        var sobelY = gray.Clone();

        Cv2.Scharr(gray, sobelY, image.Depth(), 0, 1);
        Cv2.Scharr(gray, sobelX, image.Depth(), 1, 0);

        var helper = new ImageSeamAssistant(computed, sobelX, percentage);

        if (flipOrientation)
        {
            computed = helper.ScaleX();

            helper.Source = computed;
            helper.Weighted = sobelX;
            computed = helper.ScaleY();
        }
        else
        {
            computed = helper.ScaleY();

            helper.Source = computed;
            helper.Weighted = sobelY;
            computed = helper.ScaleX();
        }


        computed = helper.Resize(computed);

        computed.SaveImage(output);

        inputImage.Dispose();

        return Task.CompletedTask;
    }
}