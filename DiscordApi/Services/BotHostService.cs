using DiscordApi.Data;
using DiscordApi.DiscordHost.Bot;
using DiscordApi.Models;
using Serilog;

namespace DiscordApi.Services;

/// <summary>
///     Service class maintaining several bot instances
/// </summary>
public class BotHostService : IHostedService, IBotHostService
{
    private readonly IServiceProvider _provider;
    private readonly IServiceScopeFactory _scopeFactory;

    public BotHostService(IServiceScopeFactory scopeFactory, IServiceProvider provider)
    {
        _scopeFactory = scopeFactory;
        _provider = provider;
    }

    /// <summary>
    ///     List of Bots - This makes the individual instances accessible by accessing the service.
    /// </summary>
    public List<Bot> Bots { get; private set; }

    /// <summary>
    ///     List of Bot Wrappers to be managed.
    /// </summary>
    public List<DiscordBot> BotWrappers { get; set; } = new();

    /// <summary>
    ///     StartAsync method spinning up all instances in the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            Bots = context.Bots.ToList();

#if DEBUG
            var bots = Bots.Where(x => x.IsDebug);
            foreach (var bot in bots)
            {
                Log.Information("Starting Bot {BotName}", bot.Name);
                DiscordBot tmp = new(bot.Name, bot.Token, _provider, bot.Presence);
                BotWrappers.Add(tmp);
                Task.Run(() => tmp.StartBot());
            }

#elif RELEASE
            foreach (var bot in Bots.Where(x => x.IsActive))
            {
                Log.Information("Starting Bot {BotName}", bot.Name);
                DiscordBot tmp =
                    new(bot.Name, bot.Token, _provider, bot.Presence);
                BotWrappers.Add(tmp);
                Task.Run(() => tmp.StartBot());
            }
#endif
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}