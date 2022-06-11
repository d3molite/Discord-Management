using OpenCvSharp;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public class ImageManipulationHelper
{
    public enum MirrorDirection
    {
        LTR,
        RTL,
        TTB,
        BTT
    }

    private static readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");

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
        else
        {
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

    public static Task DeepFry(Image img)
    {
        using var image = new Mat(img.SourcePath);

        ColorContrast(image, 50);
        Jpegify(image, 1);
        Sharpen(image);
        ColorContrast(image, 50);
        Jpegify(image, 1);
        Jpegify(image, 1);
        Jpegify(image, 1);

        img.Extension = "jpg";

        image.SaveImage(img.TargetPath);

        image.Dispose();

        return Task.CompletedTask;
    }

    public static Task MoreJpeg(Image img)
    {
        using var image = new Mat(img.SourcePath);

        var originalSize = image.Size();

        var smol = new Size(image.Width / 3, image.Height / 3);

        Cv2.Resize(image, image, smol);

        Jpegify(image, 0);

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
        Jpegify(image, 100);
        
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

    private static void Jpegify(Mat input, int quality)
    {
        var flags = new[] { (int)ImwriteFlags.JpegQuality, quality };
        var encoded = input.ImEncode(".jpg", flags);
        input = Cv2.ImDecode(encoded, ImreadModes.Color);
    }

    private static void RotateQuadrant(int slice, Mat quadrant)
    {
        for (var i = slice; i > 0; i--)
        {
            Cv2.Rotate(quadrant, quadrant, RotateFlags.Rotate90Clockwise);
        }
    }

    private static void Squarify(Mat input)
    {
        if (input.Width != input.Height)
        {
            if (input.Height > input.Width)
            {
                Cv2.Resize(input, input, new Size(input.Width, input.Width));
            }
            else if (input.Width > input.Height)
            {
                Cv2.Resize(input, input, new Size(input.Height, input.Height));
            }
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
}