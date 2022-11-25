using BotModule.Core;
using DB.Models;
using DB.Repositories;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ProcessProvider;

public class ProcessProviderService : IHostedService
{
	private readonly IServiceProvider _serviceProvider;

	public ProcessProviderService(IServiceProvider provider)
	{
		_serviceProvider = provider;

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
			if (!config.IsActiveInDebug) continue;
			Log.Information("Starting Debug {BotName}", config.Name);
			var tmp = new DiscordBot(config, _serviceProvider);
			DiscordBots.Add(tmp);
			Task.Run(() => tmp.StartBot(), token);

#elif RELEASE
            if (!config.IsActiveInRelease) continue;
            
            Log.Information("Starting Release {BotName}", config.Name);
            var tmp = new DiscordBot(config, _serviceProvider);
            DiscordBots.Add(tmp);
            Task.Run(() => tmp.StartBot(), token);
#endif
		}

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		foreach (var bot in DiscordBots) Task.Run(() => bot.StopBot(), cancellationToken);

		return Task.CompletedTask;
	}
}