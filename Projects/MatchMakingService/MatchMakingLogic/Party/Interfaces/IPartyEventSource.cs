using MatchMakingLogic.Party.Models;

namespace MatchMakingLogic.Party.Interfaces;

public interface IPartyEventSource
{
    public event Action<Client> OnPlayerJoined;
    public event Action<Client> OnPlayerLeft;
    public event Action<Client> OnLeaderChanged;
    public event Action<Session> OnSessionFound;
    public event Action<Client> OnToggleReady;
}
