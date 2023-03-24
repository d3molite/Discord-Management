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
//     [SlashCommand("undo", "Fuck, go back!")]
//     public async Task undo()
//     {
//         var channel = Context.Channel;
//         var msgs = await channel.GetMessagesAsync(5).FlattenAsync();
//         var messages = msgs.ToList();
//
//         foreach (var message in messages)
//         {
//             if (message.Author.Id == Context.Client.CurrentUser.Id)
//             {
//                 await message.DeleteAsync();
//                 await RespondAsync("Done!", ephemeral: true);
//                 return;
//             }
//         }
//     }
//
//     [SlashCommand("squoosh", "Content Aware Scale Very Funny Haha.")]
//     public async Task ContentAwareScale(int power = 1)
//     {
//         var img = await TryGetImage();
//
//         if (img.Item1)
//         {
//             await DeferAsync();
//             await ImageManipulationHelper.ContentAwareScale(img.Item2, power);
//             await SendAndDelete(img.Item2);
//         }
//     }
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
//     [SlashCommand("waaw", "Vertically mirror the last image left to right")]
//     public async Task waaw()
//     {
//         await Mirror(ImageManipulationHelper.MirrorDirection.LTR);
//     }
//
//     [SlashCommand("woow", "Vertically mirror the last image right to left")]
//     public async Task woow()
//     {
//         await Mirror(ImageManipulationHelper.MirrorDirection.RTL);
//     }
//
//     [SlashCommand("haah", "Horizontally mirror the last image top to bottom")]
//     public async Task haah()
//     {
//         await Mirror(ImageManipulationHelper.MirrorDirection.TTB);
//     }
//
//     [SlashCommand("hooh", "Horizontally mirror the last image bottom to top")]
//     public async Task hooh()
//     {
//         await Mirror(ImageManipulationHelper.MirrorDirection.BTT);
//     }
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
//     [SlashCommand("mandala", "Speen.")]
//     public async Task mandala(int slice = 1)
//     {
//         var img = await TryGetImage();
//
//         if (img.Item1)
//         {
//             await DeferAsync();
//             await ImageManipulationHelper.Mandala(img.Item2, slice);
//             await SendAndDelete(img.Item2);
//         }
//     }
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
//
//     private async Task<Tuple<bool, Image>> TryGetImage()
//     {
//         var image = await CheckForImage();
//
//         if (image != null)
//         {
//             try
//             {
//                 var imageObject = await ImageManipulationHelper.Download(image);
//                 return new Tuple<bool, Image>(true, imageObject);
//             }
//             catch (Exception ex)
//             {
//                 Log.Error("Exception in Downloading Image. {exception}", ex);
//             }
//         }
//
//         await RespondAsync("No Image found.", ephemeral: true);
//
//         return new Tuple<bool, Image>(false, new Image());
//     }
//
//     private async Task SendAndDelete(Image imageObject)
//     {
//         await FollowupWithFileAsync(imageObject.TargetPath);
//         Directory.Delete(imageObject.Folder, true);
//         Directory.CreateDirectory(imageObject.Folder);
//     }
//
//     private async Task<string?> CheckForImage()
//     {
//         var channel = Context.Channel;
//         var msgs = await channel.GetMessagesAsync(5).FlattenAsync();
//         var messages = msgs.ToList();
//         messages.Reverse();
//
//         foreach (var message in msgs)
//         {
//             if (message.Embeds.Any())
//             {
//                 var images = message.Embeds.Where(embed => embed.Url != null);
//
//                 if (images != null)
//                 {
//                     var image = images.First();
//
//                     if (IsImage(image.Url))
//                     {
//                         return CleanImageUrl(image.Url);
//                     }
//                 }
//             }
//             else if (message.Attachments.Any())
//             {
//                 var images = message.Attachments.Where(attachment => attachment.Url != null);
//
//                 var image = images.First();
//
//                 if (IsImage(image.Url))
//                 {
//                     return CleanImageUrl(image.Url);
//                 }
//             }
//         }
//
//         return null;
//     }
// }

