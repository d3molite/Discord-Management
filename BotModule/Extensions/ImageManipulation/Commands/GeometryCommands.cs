using BotModule.Extensions.ImageManipulation.Core;
using Discord.Interactions;

namespace BotModule.Extensions.ImageManipulation;

public partial class ImageCommandHandler
{
    [SlashCommand("waaw", "Vertically mirror the last image left to right")]
    public async Task waaw()
    {
        var imageResult = await TryGetImage();
        if (imageResult.Success)
        {
            await DeferAsync();
            await Mirror(imageResult.Image!, MirrorDirection.LTR);
            await SendAndDelete(imageResult.Image!);
        }
    }

    [SlashCommand("woow", "Vertically mirror the last image right to left")]
    public async Task woow()
    {
        var imageResult = await TryGetImage();
        if (imageResult.Success)
        {
            await DeferAsync();
            await Mirror(imageResult.Image!, MirrorDirection.RTL);
            await SendAndDelete(imageResult.Image!);
        }
    }

    [SlashCommand("haah", "Horizontally mirror the last image top to bottom")]
    public async Task haah()
    {
        var imageResult = await TryGetImage();
        if (imageResult.Success)
        {
            await DeferAsync();
            await Mirror(imageResult.Image!, MirrorDirection.TTB);
            await SendAndDelete(imageResult.Image!);
        }
    }

    [SlashCommand("hooh", "Horizontally mirror the last image bottom to top")]
    public async Task hooh()
    {
        var imageResult = await TryGetImage();
        if (imageResult.Success)
        {
            await DeferAsync();
            await Mirror(imageResult.Image!, MirrorDirection.BTT);
            await SendAndDelete(imageResult.Image!);
        }
    }

    [SlashCommand("mandala", "Speen.")]
    public async Task mandala(int slice = 1)
    {
        var imageResult = await TryGetImage();
        if (imageResult.Success)
        {
            await DeferAsync();
            await Mandala(imageResult.Image!, slice);
            await SendAndDelete(imageResult.Image!);
        }
    }

    [SlashCommand("squoosh", "Content Aware Scale Very Funny Haha.")]
    public async Task ContentAwareScale(int percentage = 20, bool alternateMode = false)
    {
        var imageResult = await TryGetImage();
        if (imageResult.Success)
        {
            await DeferAsync();
            await ContentAwareScale(imageResult.Image!, percentage, alternateMode);
            await SendAndDelete(imageResult.Image!);
        }
    }
}