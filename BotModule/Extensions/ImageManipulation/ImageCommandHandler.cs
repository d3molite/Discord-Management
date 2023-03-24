using BotModule.Extensions.ImageManipulation.Core;
using BotModule.Extensions.ImageManipulation.Manipulator;
using BotModule.Extensions.ImageManipulation.Utils;
using Discord;
using Discord.Interactions;
using Serilog;
using Image = BotModule.Extensions.ImageManipulation.Core.Image;

namespace BotModule.Extensions.ImageManipulation;

public partial class ImageCommandHandler : InteractionModuleBase
{
    private readonly List<string> _formats = new() { "jpg", "png", "bmp", "jpeg", "webp", "gif" };

    private static async Task Mirror(Image image, MirrorDirection direction)
    {
        if (image.IsAnimated)
        {
            GifSpool.Unroll(image);

            for (var i = 0; i < image.Frames; i++)
                await ImageManipulator.Mirror(image.FramePath(i), image.ModifiedFramePath(i), direction);

            image.IsManipulated = true;

            GifSpool.Roll(image, true);
        }

        else
        {
            await ImageManipulator.Mirror(image.SourcePath, image.TargetPath, direction);
        }
    }

    private static async Task Mandala(Image image, int slice)
    {
        if (image.IsAnimated)
        {
            GifSpool.Unroll(image);

            for (var i = 0; i < image.Frames; i++)
                await ImageManipulator.Mandala(image.FramePath(i), image.ModifiedFramePath(i), slice);

            image.IsManipulated = true;

            GifSpool.Roll(image, true);
        }

        else
        {
            await ImageManipulator.Mandala(image.SourcePath, image.TargetPath, slice);
        }
    }

    private static async Task ContentAwareScale(Image image, int percentage, bool flip)
    {
        if (image.IsAnimated)
        {
            GifSpool.Unroll(image);

            if (image.Frames < 20)
                for (var i = 0; i < image.Frames; i++)
                    await ImageManipulator.ContentAwareScale(image.FramePath(i), image.ModifiedFramePath(i), percentage,
                        flip);
            else
                Parallel.For(0, image.Frames, async i =>
                {
                    await ImageManipulator.ContentAwareScale(image.FramePath(i), image.ModifiedFramePath(i), percentage,
                        flip);
                });

            image.IsManipulated = true;

            GifSpool.Roll(image, true);
        }

        else
        {
            await ImageManipulator.ContentAwareScale(image.SourcePath, image.TargetPath, percentage, flip);
        }
    }

    private string CleanImageUrl(string url)
    {
        return url.Split("?").First();
    }

    private bool IsImage(string url)
    {
        var imageExt = CleanImageUrl(url).Split('.').Last().ToLower();
        return _formats.Contains(imageExt);
    }

    private async Task SendAndDelete(Image imageObject)
    {
        try
        {
            await FollowupWithFileAsync(imageObject.TargetPath);
        }
        catch (Exception ex)
        {
            await FollowupAsync("The resulting file was too large.", ephemeral: true);
        }

        Directory.Delete(imageObject.SubFolder, true);
    }

    private async Task<ImageResult> TryGetImage()
    {
        var image = await GetImageLink();

        if (image is null)
        {
            await RespondAsync("No Image found.", ephemeral: true);
            return ImageResult.Failed();
        }

        try
        {
            var imageObject = await ImageDownloader.Download(image);
            return ImageResult.Ok(imageObject);
        }
        catch (Exception ex)
        {
            Log.Error("Exception in Downloading Image. {exception}", ex);
            await RespondAsync("Error downloading image.");
            return ImageResult.Failed();
        }
    }

    private async Task<string?> GetImageLink()
    {
        var channel = Context.Channel;
        var messages = (await channel.GetMessagesAsync(10).FlattenAsync()).ToList();

        foreach (var message in messages)
        {
            if (message.Embeds.Any())
            {
                var images = message.Embeds.Where(embed => embed.Url != null).ToArray();

                if (images.Any())
                {
                    var image = images.First();
                    if (IsImage(image.Url)) return CleanImageUrl(image.Url);
                }
            }

            if (message.Attachments.Any())
            {
                var images = message.Attachments.Where(attachment => attachment.Url != null);

                var image = images.First();

                if (IsImage(image.Url)) return CleanImageUrl(image.Url);
            }
        }

        return string.Empty;
    }
}