using System.Timers;
using DiscordApi.Data;
using DiscordApi.DiscordHost.Bot;
using DiscordApi.Models;
using DiscordApi.REST;
using Timer = System.Timers.Timer;

namespace DiscordApi.Services;

public class UserStateService
{
    private readonly Timer _saveTimer;
    private string _saveStatus;
    private string? _selectedConfig;
    private string? _selectedGuild;

    public UserStateService()
    {
        Db = AppDBContext.Get();
        _saveTimer = new Timer(2000);
        _saveTimer.Elapsed += SaveChanges;
    }

    public DiscordBot? SelectedBot { get; private set; }
    public BotConfig ConfigItem { get; set; }

    public Guild GuildItem { get; set; }

    public bool Saving { get; set; }
    public AppDBContext Db { get; set; }

    public string SaveStatus
    {
        get => _saveStatus;
        private set
        {
            _saveStatus = value;
            OnChange?.Invoke();
        }
    }

    public string SelectedGuild
    {
        get => _selectedGuild;
        set
        {
            _selectedGuild = value;
            OnChange?.Invoke();
        }
    }

    public string SelectedConfig
    {
        get => _selectedConfig;
        set
        {
            _selectedConfig = value;
            OnChange?.Invoke();
        }
    }

    private void SaveChanges(object? sender, ElapsedEventArgs e)
    {
        Db.SaveChanges();
        _saveTimer.Stop();
        SaveStatus = $"Changes Saved {DateTime.Now:HH:mm:ss}";
    }

    public event Action OnChange;

    public async Task<bool> TryLogin(string username, string password)
    {
        Token = await AuthenticationHelper.TryAuthenticate(username, password);

        if (string.IsNullOrEmpty(Token))
        {
            LoggedIn = false;
            return false;
        }

        UserName = username;
        LoggedIn = true;

        OnChange?.Invoke();
        return true;
    }

    public async Task<bool> TryToken()
    {
        var user = await AuthenticationHelper.TryKeepAuthenticated(Token!);

        if (user == UserName) return true;

        LoggedIn = false;
        Token = "";
        return false;
    }

    public void SetBot(DiscordBot bot)
    {
        SelectedBot = bot;
        OnChange?.Invoke();
    }

    public void TriggerSave()
    {
        SaveStatus = "Saving ...";
        _saveTimer.Start();
    }

    #region Authentication

    public bool LoggedIn { get; set; }
    public string? Token { get; set; }
    public string? UserName { get; set; }

    #endregion
}