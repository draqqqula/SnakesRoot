using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Systems.ViewPort.Interfaces;

namespace SnakeGame.Systems.ViewPort;

internal class TrackingAggregator(IEnumerable<ITrackingSource> Sources)
{
    public IEnumerable<int> GetTracked(ClientIdentifier id)
    {
        var all = new List<int>();
        foreach (var source in Sources)
        {
            all.AddRange(source.GetTracked(id));
        }
        return all;
    }
}
