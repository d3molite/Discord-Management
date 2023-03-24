using BotModule.Extensions.ImageManipulation.Core;
using ImageMagick;
using ImageMagick.Formats;
using OpenCvSharp;
using Serilog;

namespace BotModule.Extensions.ImageManipulation.Utils;

public static class ImageDownloader
{
    private static readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");

    public static async Task<Image> Download(string url)
    {
        var extension = url.Split('.').Last();

        var image = new Image("image", extension, _path);

        using var client = new HttpClient();

        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

        if (!Directory.Exists(image.SubFolder)) Directory.CreateDirectory(image.SubFolder);

        await using var s = await client.GetStreamAsync(new Uri(url));

        await using var fs = new FileStream(image.SourcePath, FileMode.Create);

        await s.CopyToAsync(fs);

        if (image.Extension == "webp")
            ConvertToJpeg(image);

        ResizeToManageable(image);

        return image;
    }

    private static void ResizeToManageable(Image img)
    {
        try
        {
            using var mat = new Mat(img.SourcePath);

            if (mat.Width <= 1000) return;

            Log.Debug("Image was > 1000px Wide. Resizing");
            var scaleFactor = 1000.0 / mat.Width;
            var height = mat.Height * scaleFactor;

            Cv2.Resize(mat, mat, new Size(1000.0, height));

            mat.SaveImage(img.SourcePath);
            mat.Dispose();
        }
        catch (Exception ex)
        {
            Log.Error("Exception in Resizer {exception}", ex);
        }
    }

    private static void ConvertToJpeg(Image image)
    {
        using var img = new MagickImage(image.SourcePath);

        var defines = new JpegWriteDefines();

        image.Extension = "jpg";

        img.Write(new FileInfo(image.SourcePath));
    }
}