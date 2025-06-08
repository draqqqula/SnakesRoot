using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeCore.MathExtensions.Hexagons;

public static class HexagonOperations
{
    public static IEnumerable<HexagonTile> Inside(this HexagonGrid grid, Vector2 Position, float Radius)
    {
        var topCornerTile = GetPole(Position.Y, Radius, true, grid.SegmentHeight);
        var bottomCornerTile = GetPole(Position.Y, Radius, false, grid.SegmentHeight);
        var topBoundTile = MathF.Ceiling(Position.Y / grid.SegmentHeight);
        for (var i = topCornerTile + 1; i <= bottomCornerTile; i += 3)
        {
            var isTopHalf = i > topBoundTile;
            var factor = isTopHalf ? -1 : 1;
            var y = i - factor;
            var isBlue = Math.Abs(y % 2) != 0;
            var mapY = Convert.ToInt32(RoundConditional((float)(y - factor) / 3, !isTopHalf)) + factor;

            var side = GetHorizontalSpan(Position, grid.SegmentHeight, grid.InscribedRadius, y, Radius, mapY, isBlue);
            var pointy = GetHorizontalSpan(Position, grid.SegmentHeight, grid.InscribedRadius, y - factor, Radius, mapY, !isBlue);


            for (var offsetX = 0; offsetX < side.Count && offsetX <= pointy.Count; offsetX++)
            {
                yield return new HexagonTile() { Q = mapY, R = Math.Max(side.Start, pointy.Start) + offsetX };
            }
        }
    }

    public static IEnumerable<HexagonTile> TouchingRectangle(this HexagonGrid grid, Vector2 position, Vector2 size)
    {
        var top = Convert.ToInt32(RoundConditional((position.Y - grid.SegmentHeight * 2) / (grid.SegmentHeight * 3), false));
        var bottom = Convert.ToInt32(RoundConditional((position.Y + size.Y + grid.SegmentHeight * 2) / (grid.SegmentHeight * 3), true));

        for (var i = top; i <= bottom; i++)
        {
            var isBlue = Math.Abs(i % 2) != 0 ? 0 : -1;
            var left = Convert.ToInt32(RoundConditional((position.X - isBlue * grid.InscribedRadius) / (grid.InscribedRadius * 2), true));
            var right = Convert.ToInt32(RoundConditional((position.X + size.X - isBlue * grid.InscribedRadius) / (grid.InscribedRadius * 2), true));
            for (var j = left; j <= right; j++)
            {
                yield return new HexagonTile()
                {
                    Q = i,
                    R = j - i / 2
                };
            }
        }
    }

    public static HexagonTile GetTile(this HexagonGrid grid, Vector2 position)
    {
        var virtualX = position.X;
        var virtualY = position.Y;

        var tileY = GetSegment(virtualY, 1 / grid.SegmentHeight, 0);
        var tileX = GetSegment(virtualX, 1 / grid.InscribedRadius, 0);

        var isOrange = (tileY - 1) % 3 == 0;

        var segmentY = Convert.ToInt32(MathF.Floor((float)(tileY - 1) / 3)) + 1;

        if (isOrange && CalculateOnPointy(virtualX, virtualY, tileX, tileY, grid.InscribedRadius, grid.SegmentHeight))
        {
            segmentY -= 1;
        }

        var segmentX = GetSegment(virtualX, 1 / (grid.InscribedRadius * 2), grid.InscribedRadius - segmentY * grid.InscribedRadius);

        return new HexagonTile()
        {
            Q = segmentY,
            R = segmentX
        };
    }

    public static IEnumerable<int> GetIndexes(this HexagonBitMap map, IEnumerable<HexagonTile> tiles)
    {
        foreach (var tile in tiles)
        {
            if (map.TryGetIndex(tile.Q, tile.R, out var index))
            {
                yield return index;
            }    
        }
    }

    private static float RoundConditional(float number, bool down)
    {
        return down ? MathF.Floor(number) : MathF.Ceiling(number);
    }

    private static float GetPole(float y, float radius, bool down, float segmentLength)
    {
        var factor = down ? 1 : -1;
        var top = y - radius * factor;
        var tileY = RoundConditional(top / segmentLength, down);
        var topVertexY = RoundConditional((tileY - factor) / 3, down) + factor;
        var topCornerTile = topVertexY * 3 + 2 * factor;
        return topCornerTile;
    }

    private static Span GetHorizontalSpan(Vector2 position, float segmentHeight, float segmentWidth, float i, float radius, float mapY, bool isBlue)
    {
        var sideY = i * segmentHeight - position.Y;
        var sideX = -MathF.Sqrt(MathF.Pow(radius, 2) - MathF.Pow(sideY, 2)) + position.X;
        var sideLeftCorner = MathF.Ceiling((sideX - (isBlue ? segmentWidth : 0)) / (segmentWidth * 2)) * segmentWidth * 2 + (isBlue ? segmentWidth : 0);
        var sideSlice = MathF.Floor(((position.X - sideX) * 2 - sideLeftCorner + sideX) / (segmentWidth * 2));

        var sideMapX = GetSegment(sideLeftCorner, 1 / (segmentWidth * 2), segmentWidth - mapY * segmentWidth);

        return new Span()
        {
            Start = sideMapX,
            Count = sideSlice
        };
    }

    private static bool CalculateOnPointy(float x, float y, float tileX, float tileY, float segmentWidth, float segmentHeight)
    {
        var x0 = tileX * segmentWidth + segmentWidth / 2;
        var y0 = tileY * segmentHeight + segmentHeight / 2;
        var x1 = (x - x0) / (segmentWidth / 2);
        var y1 = (y - y0) / (segmentHeight / 2);

        var isBlue = Math.Abs((tileY - 1) % 6) <= 2;

        if (isBlue != (tileX % 2 == 0))
        {
            return x1 > y1;
        }
        else
        {
            return x1 < -y1;
        }
    }

    private static int GetSegment(float x, float factor, float offset)
    {
        return Convert.ToInt32(MathF.Floor((x + offset) * factor));
    }

    readonly struct Span
    {
        public required int Start { get; init; }
        public required float Count { get; init; }
    }
}
