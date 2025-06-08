using MatchMakingLogic.Party.Models;

namespace MatchMakingService.Services.Interfaces;

public interface IPartyMemberDialogue
{
    public Task StartAsync(PartyCode code, Client member);
}
