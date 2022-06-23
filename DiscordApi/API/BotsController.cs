using System.Text.Json;
using DiscordApi.REST;
using DiscordApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiscordApi.API;

[ApiController]
[Route("api/[controller]")]
public class BotsController : Controller
{
    private readonly BotHostService _botHostService;

    public BotsController(BotHostService botHostService)
    {
        _botHostService = botHostService;
    }

    [HttpGet("Bots")]
    public ActionResult Bots()
    {
        var isAuthorized = Task.Run(() => AuthenticationHelper.IsAuthenticated(Request)).Result;

        if (isAuthorized)
        {
            List<Dictionary<string, string>> returner = new();

            foreach (var bot in _botHostService.BotWrappers)
            {
                Dictionary<string, string> data = new();

                data.Add("name", bot.Name);
                data.Add("status", bot.Status);
                data.Add("avatar", bot.AvatarUrl);
                data.Add("servers", string.Join(Environment.NewLine, bot.Servers));
                returner.Add(data);
            }

            return Ok(JsonSerializer.Serialize(returner));
        }

        return Unauthorized();
    }
}