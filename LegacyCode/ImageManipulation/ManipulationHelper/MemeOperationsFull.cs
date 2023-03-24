/*using OpenCvSharp;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public static partial class ImageManipulationHelper
{
    public static Task DeepFry(Image img)
    {
        using var immutable = new Mat(img.SourcePath);

        var image = immutable.Clone();

        ColorContrast(image, 50);
        Jpegify(image, out image, 1);
        Sharpen(image);
        ColorContrast(image, 50);
        Jpegify(image, out image, 1);
        Jpegify(image, out image, 1);
        Jpegify(image, out image, 1);

        img.Extension = "jpg";

        image.SaveImage(img.TargetPath);

        image.Dispose();

        return Task.CompletedTask;
    }

    public static Task MoreJpeg(Image img)
    {
        using var immutable = new Mat(img.SourcePath);
        var image = immutable.Clone();

        var originalSize = image.Size();

        var smol = new Size(image.Width / 3, image.Height / 3);

        Cv2.Resize(image, image, smol);

        Jpegify(image, out image, 0);

        Cv2.Resize(image, image, originalSize);

        img.Extension = "jpg";

        image.SaveImage(img.TargetPath);

        image.Dispose();

        return Task.CompletedTask;
    }

    public static Task ContentAwareScale(Image img, int power = 1)
    {
        using var inputImage = new Mat(img.SourcePath);

        var image = ResizeToSquashable(inputImage);
        Jpegify(image, out image, 100);

        var computed = image.Clone();

        if (power > 5) power = 5;

        for (var i = 0; i <= power; i++)
        {
            var gray = computed.Clone();
            Cv2.CvtColor(computed, gray, ColorConversionCodes.BGR2GRAY);

            var sobelX = gray.Clone();
            var sobelY = gray.Clone();

            Cv2.Scharr(gray, sobelX, image.Depth(), 1, 0);
            Cv2.Scharr(gray, sobelY, image.Depth(), 0, 1);

            var helper = new ImageSeamHelper(computed, sobelX);
            computed = helper.ScaleY();

            helper.Source = computed;
            helper.Weighted = sobelY;
            computed = helper.ScaleX();

            computed = helper.Resize(computed);
        }

        computed.SaveImage(img.TargetPath);

        inputImage.Dispose();

        return Task.CompletedTask;
    }
}*/

