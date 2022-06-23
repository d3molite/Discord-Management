using System.Text.Json;
using DiscordApi.REST;
using Microsoft.AspNetCore.Mvc;

namespace DiscordApi.API;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    [HttpPost("Authenticate")]
    public ActionResult Authenticate()
    {
        var headers = Request.Headers;

        if (headers.TryGetValue("Authorization", out var auth))
        {
            var st = auth.ToString();
            var token = Task.Run(() => AuthenticationHelper.TryAuthenticate(st)).Result;

            if (!string.IsNullOrEmpty(token))
            {
                var data = new Dictionary<string, string>();
                data.Add("token", token);
                return Ok(JsonSerializer.Serialize(data));
            }
        }

        return Unauthorized();
    }

    [HttpPost("Validate")]
    public ActionResult Validate()
    {
        var headers = Request.Headers;

        if (headers.TryGetValue("Authorization", out var auth))
        {
            var token = auth.ToString().Split(' ')[1];
            var user = Task.Run(() => AuthenticationHelper.TryKeepAuthenticated(token)).Result;

            if (!string.IsNullOrEmpty(user))
            {
                var data = new Dictionary<string, string>();
                data.Add("user", user);
                return Ok(JsonSerializer.Serialize(data));
            }
        }

        return Unauthorized();
    }

    [HttpPost("Revoke")]
    public ActionResult Revoke()
    {
        var headers = Request.Headers;

        if (headers.TryGetValue("Authorization", out var auth))
        {
            var token = auth.ToString().Split(' ')[1];
            var ok = Task.Run(() => AuthenticationHelper.RevokeAuthentication(token)).Result;

            if (ok)
            {
                return Ok();
            }
        }

        return NotFound();
    }
}