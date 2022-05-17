using System.Diagnostics;
using System.Net;
using OpenCvSharp;
using Serilog;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation
{
    public class ImageManipulationHelper
    {
        private static readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");

        public enum MirrorDirection
        {
            LTR,
            RTL,
            TTB,
            BTT,
        }

        public static async Task<Image> Download(string url)
        {
            var image = new Image();
            image.Extension = url.Split('.').Last();
            image.Filename = "image";
            image.Folder = _path;

            using (HttpClient client = new HttpClient())
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
            int width = image.Width;
            int height = image.Height;
            int halfWidth = width / 2;
            int halfHeight = height / 2;

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

        private static void Jpegify(Mat input, int quality)
        {
            var flags = new int[] { (int)ImwriteFlags.JpegQuality, quality };
            var encoded = input.ImEncode(".jpg", flags);
            input = Cv2.ImDecode(encoded, ImreadModes.Color);
        }

        private static void Sharpen(Mat input)
        {
            Int16[] m = { 
                -1, -1, -1, 
                -1, 9, -1, 
                -1, -1, -1, 
            };
            Mat kernel = new Mat(rows: 3, cols: 3, type: MatType.CV_16SC1, data: m);
            Cv2.Filter2D(input, input, -1, kernel);
        }

        private static void Noise(Mat input)
        {
            Int16[] m = {
                2, -2, 2,
                -2, 1, -2,
                2, -2, 2,
            };
            Mat kernel = new Mat(rows: 3, cols: 3, type: MatType.CV_16SC1, data: m);
            Cv2.Filter2D(input, input, -1, kernel);
        }

        private static void ColorContrast(Mat input, int factor)
        {
            if (input.Channels() == 3)
            {
                var mat3 = new Mat<Vec3b>(input);

                Cv2.CvtColor(mat3, mat3, ColorConversionCodes.BGR2HSV);

                var indexer = mat3.GetGenericIndexer<Vec3b>();

                for (int y = 0; y < mat3.Height; y++)
                {
                    for (int x = 0; x < mat3.Width; x++)
                    {
                        Vec3b color = indexer[y, x];
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

                for (int y = 0; y < mat3.Height; y++)
                {
                    for (int x = 0; x < mat3.Width; x++)
                    {
                        Vec4b color = indexer[y, x];
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
            else
            {
                return Convert.ToByte(input + factor);
            }
        }

        private static void ResizeToManageable(Image img)
        {
            try
            {
                using var mat = new Mat(img.SourcePath);

                if (mat.Width > 1000)
                {
                    Log.Debug("Image was > 1000px Wide. Resizing");
                    var scaleFactor = (1000.0 / mat.Width);
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
    }
}
