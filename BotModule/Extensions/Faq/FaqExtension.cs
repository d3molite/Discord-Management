using BotModule.DI;
using DB.Models.Objects;
using DB.Repositories;
using Discord;
using Discord.Interactions;
using Strings.Manager;

namespace BotModule.Extensions.Faq;

public class FaqExtension : InteractionModuleBase
{
    private readonly ILanguageProvider _languageProvider;

    public FaqExtension(ILanguageProvider languageProvider)
    {
        _languageProvider = languageProvider;
    }

    [EnabledInDm(false)]
    [SlashCommand("faq", "Enter a keyword and press enter!")]
    public async Task Faq(string keyword = "")
    {
        if (string.IsNullOrEmpty(keyword)) await RespondWithHelp();

        var config = FaqConfigRepository.Get(Context.Guild.Id);

        if (config is null) return;

        var item = GetMatch(keyword, config.FaqItems);

        if (item is null)
        {
            await RespondWithHelp(true);
            return;
        }

        await RespondWithFaq(item);
    }

    private async Task RespondWithFaq(FaqItem item)
    {
        var embed = GenerateFaq(item);
        var message = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Faq, "help_faq");

        await RespondAsync(message, new[] { embed });
    }

    private async Task RespondWithHelp(bool isError = false)
    {
        var id = Context.Guild.Id;
        var embed = GenerateHelp(id);

        var message = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Faq, "help_message");
        if (isError) message = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Faq, "help_error");

        await RespondAsync(message, new[] { embed });
    }

    private static FaqItem? GetMatch(string keyword, IEnumerable<FaqItem> items)
    {
        return items.FirstOrDefault(x => x.Triggers.Split(",").Contains(keyword.ToLower()));
    }

    private Embed GenerateHelp(ulong guildId)
    {
        var config = FaqConfigRepository.Get(guildId);

        var builder = new EmbedBuilder();

        var sb = new List<string>();

        foreach (var item in config!.FaqItems) sb.Add($"{GetCapitalized(item.Triggers.Split(",").First())}");

        var message = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Faq, "help_title");
        builder.AddField(message, string.Join(", ", sb));

        return builder.Build();
    }

    private Embed GenerateFaq(FaqItem item)
    {
        var embed = new EmbedBuilder();

        embed.AddField(item.Question, item.Response);

        if (item.MessageLink == null) return embed.Build();

        var title = _languageProvider.GetResource(Context, ResourceLookup.ResourceGroup.Faq, "help_link");
        embed.AddField(title, item.MessageLink);

        return embed.Build();
    }

    private static string GetCapitalized(string item)
    {
        return $"{item[0].ToString().ToUpper()}{item[1..]}";
    }
}