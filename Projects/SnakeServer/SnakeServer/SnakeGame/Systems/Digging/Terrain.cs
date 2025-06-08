using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using SnakeCore.MathExtensions.Hexagons;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Systems.Digging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

internal class Terrain(IMapProvider MapProvider, IGridProvider GridProvider)
{
    public IEnumerable<HexagonTile> Dig(Circle explorationCircle, Circle diggingCircle)
    {
        var terrainFound = GridProvider.Grid
            .Inside(explorationCircle.Position, explorationCircle.Radius)
            .Select(coordinates => MapProvider.Map[coordinates.Q, coordinates.R])
            .Any(IsTrue);

        if (!terrainFound)
        {
            yield break;
        }

        var diggedTiles = GridProvider.Grid
            .Inside(diggingCircle.Position, diggingCircle.Radius);

        foreach (var tile in diggedTiles)
        {
            if (IsTrue(MapProvider.Map[tile]))
            {
                MapProvider.Map[tile] = false;
                yield return tile;
            }
        }
    }

    private bool IsTrue(bool? nullable)
    {
        return nullable.HasValue && nullable.Value;
    }
}