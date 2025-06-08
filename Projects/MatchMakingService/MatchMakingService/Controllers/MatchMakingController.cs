using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MatchMakingService.Controllers;

[Route("matchmaking")]
public class MatchMakingController : Controller
{
    private readonly string[] _hosts;
    public MatchMakingController(IConfiguration configuration)
    {
        _hosts = configuration.GetSection("HostList").Get<string[]>();
    }

    [Route("find")]
    [HttpGet]
    public async Task<IActionResult> Find()
    {
        var mainHost = _hosts.FirstOrDefault();
        var client = new HttpClient();
        var result = await client.GetAsync($"https://{mainHost}/all");
        using var stream = await result.Content.ReadAsStreamAsync();
        var sessions = await JsonSerializer.DeserializeAsync<Guid[]>(stream);
        if (sessions is not null && sessions.Any())
        {
            return Redirect($"https://{mainHost}/echo?id={sessions.First()}");
        }

        return Redirect($"https://{mainHost}/launch");
    }
}
