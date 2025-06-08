using ServerEngine.Models;

namespace SnakeGame.Systems.Service;

internal interface IClientRegistry
{
    public IEnumerable<ClientIdentifier> All { get; }
    public IEnumerable<ClientIdentifier> Online { get; }
}
