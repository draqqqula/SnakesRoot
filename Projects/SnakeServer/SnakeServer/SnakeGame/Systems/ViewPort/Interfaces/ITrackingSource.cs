using ServerEngine.Models;

namespace SnakeGame.Systems.ViewPort.Interfaces;

internal interface ITrackingSource
{
    public IEnumerable<int> GetTracked(ClientIdentifier id);
}
