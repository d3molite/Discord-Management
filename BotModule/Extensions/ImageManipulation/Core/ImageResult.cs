namespace BotModule.Extensions.ImageManipulation.Core;

public class ImageResult
{
    public Image? Image { get; set; }

    public bool Success { get; set; }

    public static ImageResult Failed()
    {
        return new ImageResult
        {
            Success = false
        };
    }

    public static ImageResult Ok(Image image)
    {
        return new ImageResult
        {
            Success = true,
            Image = image
        };
    }
}