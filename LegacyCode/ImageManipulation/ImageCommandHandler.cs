// using Discord;
// using Discord.Interactions;
// using Serilog;
//
// namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;
//
// public class ImageCommandHandler : InteractionModuleBase
// {
//     private readonly List<string> _formats = new() { "jpg", "png", "bmp", "jpeg", "webp" };
//
//     private string CleanImageUrl(string url)
//     {
//         return url.Split("?").First();
//     }
//
//     private bool IsImage(string url)
//     {
//         var imageExt = CleanImageUrl(url).Split('.').Last().ToLower();
//
//         if (_formats.Contains(imageExt))
//         {
//             return true;
//         }
//
//         return false;
//     }
//
//     
//
//     [SlashCommand("memeify", "Spicy memez.")]
//     public async Task memeify(int power = 1)
//     {
//         var img = await TryGetImage();
//
//         if (img.Item1)
//         {
//             await DeferAsync();
//             await ImageManipulationHelper.Memeify(img.Item2, power);
//             await SendAndDelete(img.Item2);
//         }
//     }
//
//
//     [SlashCommand("deepfry", "Do you really need an explanation?")]
//     public async Task deepfry()
//     {
//         var img = await TryGetImage();
//
//         if (img.Item1)
//         {
//             await DeferAsync();
//             await ImageManipulationHelper.DeepFry(img.Item2);
//             await SendAndDelete(img.Item2);
//         }
//     }
//
//     [SlashCommand("needsmorejpeg", "Mmmm Crusty Compression!")]
//     public async Task needsmorejpeg()
//     {
//         var img = await TryGetImage();
//
//         if (img.Item1)
//         {
//             await DeferAsync();
//             await ImageManipulationHelper.MoreJpeg(img.Item2);
//             await SendAndDelete(img.Item2);
//         }
//     }
//
//
//     private async Task Mirror(ImageManipulationHelper.MirrorDirection direction)
//     {
//         var img = await TryGetImage();
//
//         if (img.Item1)
//         {
//             await DeferAsync();
//             await ImageManipulationHelper.Mirror(img.Item2, direction);
//             await SendAndDelete(img.Item2);
//         }
//     }

