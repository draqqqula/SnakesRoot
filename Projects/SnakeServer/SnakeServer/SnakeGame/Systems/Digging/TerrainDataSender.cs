using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Mechanics.ViewPort;
using SnakeGame.Systems.Digging.Interfaces;
using SnakeCore.MathExtensions.Hexagons;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Systems.Collision;
using Newtonsoft.Json.Linq;

namespace SnakeGame.Systems.Digging;

internal class TerrainDataSender(
    IRuntimeCommandFactory RuntimeCommandFactory,
    IGridProvider GridProvider,
    IMapProvider MapProvider,
    Dictionary<ClientIdentifier, ClientViewPort> ViewPorts,
    MapPictureRepository Repository
    )
{
    private readonly RuntimeCommand<int[]> _breakTilesCommand = new RuntimeCommand<int[]>("BreakTiles", RuntimeCommandFactory);
    private readonly RuntimeCommand<int[]> _removeTilesCommand = new RuntimeCommand<int[]>("RemoveTiles", RuntimeCommandFactory);
    private readonly Dictionary<ClientViewPort, AABB> _cachedLayouts = [];

    public void UpdateForClients(IEnumerable<HexagonTile> broken)
    {
        var brokenIndex = MapProvider.Map.GetIndexes(broken);
        Repository.Update(brokenIndex);

        foreach (var (client, viewPort) in ViewPorts)
        {
            Repository.Pictures[client].AddTiles(brokenIndex);
            UpdateDiscovered(client, viewPort, brokenIndex);
            UpdateObserved(client, viewPort, broken);
        }
    }

    private void UpdateObserved(ClientIdentifier client, ClientViewPort viewPort, IEnumerable<HexagonTile> broken)
    {
        var observed = SelectObserved(viewPort, broken);
        var observedIndexes = MapProvider.Map.GetIndexes(observed);
        var result = Repository.Pictures[client].RemoveTiles(observedIndexes).ToArray();
        SendOrIgnore(_breakTilesCommand, client, result);
    }

    private IEnumerable<HexagonTile> SelectObserved(ClientViewPort viewPort, IEnumerable<HexagonTile> broken)
    {
        var body = viewPort.GetBody().First();
        foreach (var tile in broken)
        {
            var bounds = GridProvider.Grid.GetBounds(tile);
            if (body.Intersects(bounds))
            {
                yield return tile;
            }
        }
    }

    private void UpdateDiscovered(ClientIdentifier client, ClientViewPort viewPort, IEnumerable<int> broken)
    {
        var visited = GetVisitedTiles(viewPort);
        var updated = Repository.Pictures[client].RemoveTiles(visited).ToArray();

        var brokenAndUpdated = updated.Intersect(broken).ToArray();

        SendOrIgnore(_breakTilesCommand, client, brokenAndUpdated);

        var removed = updated.Except(brokenAndUpdated).ToArray();

        SendOrIgnore(_removeTilesCommand, client, removed);
    }

    private IEnumerable<int> GetVisitedTiles(ClientViewPort viewPort)
    {
        var diff = GetDiff(viewPort);
        foreach (var rect in diff)
        {
            foreach (var tile in GridProvider.Grid.TouchingRectangle(rect.Min, rect.Size))
            {
                if (MapProvider.Map.TryGetIndex(tile.Q, tile.R, out var index))
                {
                    yield return index;
                }
            }
        }
    }

    private IEnumerable<AABB> GetDiff(ClientViewPort viewPort)
    {
        var rect = viewPort.GetBody().First();
        if (_cachedLayouts.TryGetValue(viewPort, out var cached))
        {
            foreach (var part in rect.Subtract(cached))
            {
                yield return part;
            }
        }
        else
        {
            yield return rect;
        }
        _cachedLayouts[viewPort] = rect;
    }

    private void SendOrIgnore(RuntimeCommand<int[]> command, ClientIdentifier client, int[] data)
    {
        if (data.Length != 0)
        {
            command.Send(client, data);
        }
    }
}
