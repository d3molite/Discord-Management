namespace DiscordApi.Services;

public class UserStateService
{
    public event Action OnChange;
    public bool LoggedIn { get; set; }
    public string Token { get; set; }
    public string UserName { get; set; }
    public void TryLogin(string username, string password)
    {
        UserName = username;
        LoggedIn = true;
        OnChange?.Invoke();
    }
}