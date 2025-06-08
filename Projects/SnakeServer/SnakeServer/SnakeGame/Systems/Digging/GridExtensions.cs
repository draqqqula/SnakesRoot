using MessageSchemes;
using SnakeCore.MathExtensions.Hexagons;
using SnakeGame.Mechanics.Collision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

internal static class GridExtensions
{
    public static AABB GetBounds(this HexagonGrid grid, HexagonTile tile)
    {
        var position = grid.Translate(tile);
        var size = new Vector2(grid.InscribedRadius, grid.SegmentHeight * 2);
        return new AABB()
        {
            Min = position - size,
            Max = position + size,
        };
    }
}
