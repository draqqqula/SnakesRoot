using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakingLogic.Party.Implementation;

internal class PartyEventSource : IPartyEventSource
{
    public event Action<Client>? OnPlayerJoined;
    public event Action<Client>? OnPlayerLeft;
    public event Action<Client>? OnLeaderChanged;
    public event Action<Session>? OnSessionFound;
    public event Action<Client>? OnToggleReady;

    public void BroadcastJoined(Client client)
    {
        OnPlayerJoined?.Invoke(client);
    }

    public void BroadcastLeft(Client client) 
    {
        OnPlayerLeft?.Invoke(client);
    }

    public void BroadcastLeaderChanged(Client client)
    {
        OnLeaderChanged?.Invoke(client);
    }

    public void BroadcastSessionFound(Session session)
    {
        OnSessionFound?.Invoke(session);
    }

    public void BroadcastToggleReady(Client client)
    {
        OnToggleReady?.Invoke(client);
    }
}
