using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;

namespace SnakeGame.Systems.Service;

internal class ClientRegistry : ISessionService, IClientRegistry
{
    private readonly List<ClientIdentifier> _online = [];
    private readonly List<ClientIdentifier> _all = [];

    public IEnumerable<ClientIdentifier> All => _all;
    public IEnumerable<ClientIdentifier> Online => _online;

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        _online.Add(id);
        if (!_all.Contains(id))
        {
            _all.Add(id);
        }
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        _online.Remove(id);
    }
}
