using OpenCvSharp;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public static partial class ImageManipulationHelper
{
    public enum MirrorDirection
    {
        LTR,
        RTL,
        TTB,
        BTT
    }

    private static readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");

    private static void Jpegify(Mat input, out Mat output, int quality)
    {
        var flags = new[] { (int)ImwriteFlags.JpegQuality, quality };
        var encoded = input!.ImEncode(".jpg", flags);
        output = Cv2.ImDecode(encoded, ImreadModes.Color);
    }

    public static Task Memeify(Image img, int power)
    {
        using var immutable = new Mat(img.SourcePath);

        var inputImage = immutable.Clone();

        Jpegify(inputImage, out inputImage, 1);

        if (power > 100) power = 1000;

        for (var i = 0; i < power; i++)
        {
            var mat2 = new Mat(MemeResources.GetRandom(), ImreadModes.Unchanged);
            var scale = Random.Next(3, 8);
            var angle = Random.Next(-90, 90);

            var position = GetSurroundingSections(inputImage.Width, inputImage.Height);

            mat2 = mat2.Resize(new Size(inputImage.Width / scale, inputImage.Height / scale));
            var rotate = Cv2.GetRotationMatrix2D(new Point2f(mat2.Width / 2, mat2.Height / 2), angle, 1);
            Cv2.WarpAffine(mat2, mat2, rotate, mat2.Size());

            OverlayPng(inputImage, mat2, out inputImage, position.Item1, position.Item2);
        }

        ColorContrast(inputImage, 50);
        Sharpen(inputImage);

        img.Extension = "jpg";

        inputImage.SaveImage(img.TargetPath);

        return Task.CompletedTask;
    }
}