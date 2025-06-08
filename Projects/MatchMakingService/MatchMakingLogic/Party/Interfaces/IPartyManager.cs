using MatchMakingLogic.Party.Models;

namespace MatchMakingLogic.Party.Interfaces;

public interface IPartyManager
{
    public PartyCode CreateFor(Client owner);
    public IPartyController? TryGetByCode(PartyCode code);
}
