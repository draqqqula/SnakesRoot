using MatchMakingLogic.Party.Models;

namespace MatchMakingService.Services.Interfaces;

public interface IPartyLeaderDialogue
{
    public Task StartAsync(Client owner);
}
