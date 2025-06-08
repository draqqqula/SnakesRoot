using System.Collections.Immutable;
using System.Numerics;

namespace SnakeGame.Mechanics.Collision.Shapes;

internal readonly partial record struct RotatableSquare : IFlatShape
{
    public required Vector2 Position { get; init; }
    public required float Size { get; init; }
    public required float Rotation { get; init; }
    public readonly Vector2 TopLeft
    {
        get
        {
            return RotateVertex(Position, Size / 2 * new Vector2(-1, -1), Rotation);
        }
    }
    public readonly Vector2 TopRight
    {
        get
        {
            return RotateVertex(Position, Size / 2 * new Vector2(+1, -1), Rotation);
        }
    }
    public readonly Vector2 BottomLeft
    {
        get
        {
            return RotateVertex(Position, Size / 2 * new Vector2(-1, +1), Rotation);
        }
    }
    public readonly Vector2 BottomRight
    {
        get
        {
            return RotateVertex(Position, Size / 2 * new Vector2(+1, +1), Rotation);
        }
    }

    public readonly float DiagonalLength
    {
        get
        {
            return MathF.Sqrt(MathF.Pow(Size, 2) + MathF.Pow(Size, 2));
        }
    }
}

internal partial record struct RotatableSquare : IFlatShape
{
    public Vector2[] Vertexes =>
    [
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft
    ];
    public Polygon AsPolygon()
    {
        Vector2[] edgeArray =
            [
                BottomRight - TopRight,
                BottomLeft - BottomRight,
                TopLeft - BottomLeft,
                TopRight - TopLeft
            ];
        return new Polygon()
        {
            Edges = edgeArray.ToImmutableArray(),
            Vertexes = Vertexes.ToImmutableArray()
        };
    }

    public AABB GetBounds()
    {
        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;

        foreach (var vertex in Vertexes)
        {
            if (vertex.X < minX)
            {
                minX = vertex.X;
            }
            if (vertex.Y < minY)
            {
                minY = vertex.Y;
            }
            if (vertex.X > maxX)
            {
                maxX = vertex.X;
            }
            if (vertex.Y > maxY)
            {
                maxY = vertex.Y;
            }
        }

        return new AABB()
        {
            Min = new Vector2(minX, minY),
            Max = new Vector2(maxX, maxY)
        };
    }

    public AABB GetUnrotated()
    {
        return new AABB()
        {
            Min = Position - Vector2.One * Size,
            Max = Position + Vector2.One * Size
        };
    }

    struct RectangleVertexes
    {
        public Vector2 TopLeft;
        public Vector2 TopRight;
        public Vector2 BottomLeft;
        public Vector2 BottomRight;
    }

    private static Vector2 RotateVertex(Vector2 center, Vector2 delta, float rotation)
    {
        float distance = delta.Length();
        var originalAngle = MathF.Atan2(delta.Y, delta.X);
        float rotatedX = center.X + distance * MathF.Cos(originalAngle + rotation);
        float rotatedY = center.Y + distance * MathF.Sin(originalAngle + rotation);

        return new Vector2(rotatedX, rotatedY);
    }
}