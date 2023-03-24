using OpenCvSharp;

namespace BotModule.Extensions.ImageManipulation.Manipulator;

public static partial class ImageManipulator
{
    public static void MakeJpeg(Mat input, out Mat output, int quality)
    {
        var flags = new[] { (int)ImwriteFlags.JpegQuality, quality };
        var encoded = input!.ImEncode(".jpg", flags);
        output = Cv2.ImDecode(encoded, ImreadModes.Color);
    }
}