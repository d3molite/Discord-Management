using System.Timers;
using Discord.Rest;
using Discord.WebSocket;
using Timer = System.Timers.Timer;

namespace DiscordApi.DiscordHost.Utils;

public class ReadyCheckState
{
    public Dictionary<SocketGuildUser, int> ReadyStates = new();

    public ReadyCheckState(IEnumerable<SocketGuildUser> users, string title)
    {
        foreach (var socketGuildUser in users)
        {
            ReadyStates.Add(socketGuildUser, 2);
        }

        Title = title;

        Cooldown = new Timer(30000);
        Cooldown.Elapsed += Elapsed;
        Cooldown.Start();
    }

    public RestUserMessage Message { get; set; }

    public string Title { get; set; }

    public Timer Cooldown { get; }

    private void Elapsed(object? sender, ElapsedEventArgs e)
    {
        Cooldown.Stop();
    }

    public void FinalizeStates()
    {
        foreach (var state in ReadyStates.Where(state => state.Value == 2))
        {
            ReadyStates[state.Key] = 0;
        }
    }

    public bool Success()
    {
        if (ReadyStates.Any(s => s.Value == 0))
        {
            return false;
        }

        return true;
    }

    public void UpdateState(SocketGuildUser user, int status)
    {
        if (ReadyStates.TryGetValue(user, out _))
        {
            ReadyStates[user] = status;
        }
        else
        {
            ReadyStates.Add(user, status);
        }
    }
}