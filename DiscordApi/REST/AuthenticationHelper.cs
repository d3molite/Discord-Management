using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;

namespace DiscordApi.REST;

public class AuthenticationHelper
{
    private static readonly string _authenticationUrl = "http://auth.demolite.de/";
    private static readonly RestClient client = new(_authenticationUrl);

    public static async Task<string> TryAuthenticate(string encodedAuthentication)
    {
        var request = new RestRequest("authenticate/")
            .AddHeader("Authorization", $"{encodedAuthentication}");

        try
        {
            var response = await client.PostAsync(request, CancellationToken.None);
            var rs = JObject.Parse(response.Content ?? string.Empty);
            return rs["token"]?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Log.Error("Error in Authentication Request: {Exception}", ex.Message);
            return string.Empty;
        }
    }

    public static async Task<string> TryAuthenticate(string username, string password)
    {
        var cred = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

        var request = new RestRequest("authenticate/")
            .AddHeader("Authorization", $"Basic {cred}");

        try
        {
            var response = await client.PostAsync(request, CancellationToken.None);
            var rs = JObject.Parse(response.Content ?? string.Empty);
            return rs["token"]?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Log.Error("Error in Authentication Request: {Exception}", ex.Message);
            return string.Empty;
        }
    }

    public static async Task<string> TryKeepAuthenticated(string token)
    {
        var request = new RestRequest("verify/")
            .AddHeader("Authorization", $"Bearer {token}");

        try
        {
            var response = await client.PostAsync(request, CancellationToken.None);
            var rs = JObject.Parse(response.Content ?? string.Empty);
            return rs["user"]?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Log.Error("Error in Authentication Request: {Exception}", ex.Message);
            return string.Empty;
        }
    }

    public static async Task<bool> RevokeAuthentication(string token)
    {
        var request = new RestRequest("remove/")
            .AddHeader("Authorization", $"Bearer {token}");

        try
        {
            var response = await client.PostAsync(request, CancellationToken.None);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Error in Remove Request: {Exception}", ex.Message);
            return false;
        }
    }

    public static async Task<bool> IsAuthenticated(HttpRequest request)
    {
        var headers = request.Headers;

        if (headers.TryGetValue("Authorization", out var auth))
        {
            var st = auth.ToString().Split(' ')[1];
            var user = await TryKeepAuthenticated(st);

            if (!string.IsNullOrEmpty(user))
            {
                return true;
            }
        }

        return false;
    }
}