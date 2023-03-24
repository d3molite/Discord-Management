/*using OpenCvSharp;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public static partial class ImageManipulationHelper
{
    private static readonly Random Random = new();

    public static async Task<Image> Download(string url)
    {
        var image = new Image();
        image.Extension = url.Split('.').Last();
        image.Filename = "image";
        image.Folder = _path;

        using (var client = new HttpClient())
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            using (var s = await client.GetStreamAsync(new Uri(url)))
            {
                using (var fs = new FileStream(image.SourcePath, FileMode.Create))
                {
                    await s.CopyToAsync(fs);
                }
            }
        }

        ResizeToManageable(image);
        return image;
    }

    private static void ResizeToManageable(Image img)
    {
        try
        {
            using var mat = new Mat(img.SourcePath);

            if (mat.Width > 1000)
            {
                Log.Debug("Image was > 1000px Wide. Resizing");
                var scaleFactor = 1000.0 / mat.Width;
                var height = mat.Height * scaleFactor;

                Cv2.Resize(mat, mat, new Size(1000.0, height));

                img.Filename = "r_" + img.Filename;

                mat.SaveImage(img.SourcePath);
                mat.Dispose();
            }
        }
        catch (Exception ex)
        {
            Log.Error("Exception in Resizer {exception}", ex);
        }
    }

    private static Mat ResizeToSquashable(Mat mat)
    {
        var width = 600.0;

        try
        {
            if (mat.Width > width)
            {
                var scaleFactor = width / mat.Width;
                var height = mat.Height * scaleFactor;

                Cv2.Resize(mat, mat, new Size(width, height));
            }

            return mat;
        }
        catch (Exception ex)
        {
            Log.Error("Exception in Resizer {exception}", ex);
        }

        return new Mat(mat);
    }

    private static void Squarify(Mat input)
    {
        if (input.Width == input.Height) return;

        if (input.Height > input.Width)
        {
            Cv2.Resize(input, input, new Size(input.Width, input.Width));
        }
        else if (input.Width > input.Height)
        {
            Cv2.Resize(input, input, new Size(input.Height, input.Height));
        }
    }

    private static void Sharpen(Mat input)
    {
        short[] m =
        {
            -1, -1, -1,
            -1, 9, -1,
            -1, -1, -1
        };
        var kernel = new Mat(3, 3, MatType.CV_16SC1, m);
        Cv2.Filter2D(input, input, -1, kernel);
    }

    private static void Noise(Mat input)
    {
        short[] m =
        {
            2, -2, 2,
            -2, 1, -2,
            2, -2, 2
        };
        var kernel = new Mat(3, 3, MatType.CV_16SC1, m);
        Cv2.Filter2D(input, input, -1, kernel);
    }
}*/

