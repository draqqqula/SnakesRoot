using MatchMakingLogic.Party.Implementation.Generators;
using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakingLogic.Party.Implementation;

internal class Party : IPartyController
{
    private object _locker = new object();
    private readonly List<Client> _members;
    private readonly Dictionary<Client, bool> _ready;
    private readonly PartyEventSource _eventSource;

    public event Action OnDismiss;

    public Party(Client owner)
    {
        _eventSource = new PartyEventSource();
        _members = new List<Client>() { owner };
        _ready = new Dictionary<Client, bool>() { { owner, false } };
    }

    private Client Leader => _members[0];

    public IPartyEventSource EventSource => _eventSource;

    public IEnumerable<Client> Members => _members;

    public IEnumerable<(Client, bool)> Ready => _ready.Select(kvp => (kvp.Key, kvp.Value));

    public void AddMember(Client client)
    {
        lock (_locker)
        {
            _members.Add(client);
            _ready.Add(client, false);
            _eventSource.BroadcastJoined(client);
        }
    }

    public void ConnectTo(Session session)
    {
        _eventSource.BroadcastSessionFound(session);
    }

    public void RemoveMember(Client client)
    {
        lock (_locker)
        {
            if (client == Leader)
            {
                if (_members.Count > 1)
                {
                    _eventSource.BroadcastLeaderChanged(_members[1]);
                }
                else
                {
                    OnDismiss?.Invoke();
                }
            }
            _members.Remove(client);
            _ready.Remove(client);
            _eventSource.BroadcastLeft(client);
        }
    }

    public void StartMatchMaking()
    {
        throw new NotImplementedException();
    }

    public void ToggleReady(Client client)
    {
        _eventSource.BroadcastToggleReady(client);
        _ready[client] = !_ready[client];
    }
}
