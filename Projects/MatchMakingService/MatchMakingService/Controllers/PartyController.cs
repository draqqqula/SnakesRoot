using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using MatchMakingService.Services;
using MatchMakingService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MatchMakingService.Controllers;

[Route("party")]
public class PartyController : Controller
{
    [Route("create")]
    [HttpGet]
    public async Task Create([FromQuery]Guid clientId, [FromServices]IPartyLeaderDialogue dialogue)
    {
        await dialogue.StartAsync(new Client()
        {
            Id = clientId,
        });
    }

    [Route("join")]
    [HttpGet]
    public async Task Join([FromQuery]Guid clientId, [FromQuery]string code, [FromServices] IPartyMemberDialogue dialogue)
    {
        await dialogue.StartAsync(new PartyCode()
        {
            String = code
        },
        new Client()
        {
            Id = clientId
        });
    }
}
