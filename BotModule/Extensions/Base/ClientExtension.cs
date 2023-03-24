using BotModule.DI;
using Discord.WebSocket;
using Strings.Manager;

namespace BotModule.Extensions.Base;

public class ClientExtension : Extension, IClientExtension
{
    private readonly ILanguageProvider _languageProvider;

    public ClientExtension(DiscordSocketClient client, string botName,
        ILanguageProvider languageProvider) : base(botName)
    {
        Client = client;
        _languageProvider = languageProvider;
    }

    public DiscordSocketClient Client { get; }

    public virtual string GetResource(ulong guildId, ResourceLookup.ResourceGroup resourceGroup, string resourceKey)
    {
        return _languageProvider.GetResource(guildId, Client.CurrentUser.Id, resourceGroup, resourceKey);
    }
}