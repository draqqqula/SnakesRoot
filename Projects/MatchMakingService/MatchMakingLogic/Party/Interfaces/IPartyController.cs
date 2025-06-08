using MatchMakingLogic.Party.Models;

namespace MatchMakingLogic.Party.Interfaces;

public interface IPartyController
{
    public event Action OnDismiss;
    public IPartyEventSource EventSource { get; }
    public IEnumerable<Client> Members { get; }
    public IEnumerable<(Client, bool)> Ready { get; }
    public void AddMember(Client client);
    public void RemoveMember(Client client);
    public void ToggleReady(Client client);
    public void StartMatchMaking();
    public void ConnectTo(Session session);
}
