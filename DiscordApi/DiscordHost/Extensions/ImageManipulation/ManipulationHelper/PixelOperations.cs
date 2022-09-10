using OpenCvSharp;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public static partial class ImageManipulationHelper
{
    private static void ColorContrast(Mat input, int factor)
    {
        if (input.Channels() == 3)
        {
            var mat3 = new Mat<Vec3b>(input);

            Cv2.CvtColor(mat3, mat3, ColorConversionCodes.BGR2HSV);

            var indexer = mat3.GetGenericIndexer<Vec3b>();

            for (var y = 0; y < mat3.Height; y++)
            {
                for (var x = 0; x < mat3.Width; x++)
                {
                    var color = indexer[y, x];
                    color.Item1 = Boost(color.Item1, factor);
                    color.Item2 = Boost(color.Item2, factor / 2);
                    indexer[y, x] = color;
                }
            }

            Cv2.CvtColor(mat3, mat3, ColorConversionCodes.HSV2BGR);

            input = mat3;
            mat3.Dispose();
        }
        else if (input.Channels() == 4)
        {
            var mat3 = new Mat<Vec4b>(input);

            Cv2.CvtColor(mat3, mat3, ColorConversionCodes.BGR2HSV);

            var indexer = mat3.GetGenericIndexer<Vec4b>();

            for (var y = 0; y < mat3.Height; y++)
            {
                for (var x = 0; x < mat3.Width; x++)
                {
                    var color = indexer[y, x];
                    color.Item1 = Boost(color.Item1, factor);
                    color.Item2 = Boost(color.Item2, factor / 2);
                    indexer[y, x] = color;
                }
            }

            Cv2.CvtColor(mat3, mat3, ColorConversionCodes.HSV2BGR);

            input = mat3;
            mat3.Dispose();
        }
    }

    private static byte Boost(byte input, int factor)
    {
        if (input + factor > byte.MaxValue)
        {
            return byte.MaxValue;
        }

        return Convert.ToByte(input + factor);
    }
}