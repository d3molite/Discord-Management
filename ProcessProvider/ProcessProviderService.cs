using BotModule.Core.ExtensionModules;
using DB.Models;
using DB.Repositories;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ProcessProvider;

public class ProcessProviderService : IHostedService
{
    public ProcessProviderService()
    {
        BotConfigs = BotRepository.Get().ToList();
        DiscordBots = new List<DiscordBot>();
    }

    private List<Bot> BotConfigs { get; }

    public List<DiscordBot> DiscordBots { get; }

    public Task StartAsync(CancellationToken token)
    {
        foreach (var config in BotConfigs)
        {
#if DEBUG
            if (config.IsActiveInDebug)
            {
                Log.Information("Starting {BotName}", config.Name);
                var tmp = new DiscordBot();
                DiscordBots.Add(tmp);
                Task.Run(() => tmp.StartBot());
            }

#elif RELEASE
#endif
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}