using MatchMakingLogic.Party.Models;

namespace MatchMakingLogic.Party.Interfaces;

internal interface ICodeGenerator
{
    public PartyCode New();
    public void Free(PartyCode code);
}
