using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Services.Output;
using SnakeGame.Services.Output.Commands;
using SnakeGame.Systems.ViewPort.Interfaces;

namespace SnakeGame.Services.Gameplay;

internal class MinimapManager : ITrackingSource
{
    private Dictionary<ClientIdentifier, List<int>> Pinned = [];
    public IEnumerable<int> GetTracked(ClientIdentifier id)
    {
        if (Pinned.TryGetValue(id, out var result))
        {
            return result;
        }
        return [];
    }

    public void Pin(ClientIdentifier clientId, CommandSender sender, params int[] frames)
    {
        PinIconCommand.To(clientId, sender, frames);
        if (Pinned.TryGetValue(clientId, out var collection))
        {
            collection.AddRange(frames);
            return;
        }
        Pinned.Add(clientId, frames.ToList());
    }
}
