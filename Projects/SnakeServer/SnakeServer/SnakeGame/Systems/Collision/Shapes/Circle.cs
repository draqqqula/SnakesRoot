using System.Collections.Immutable;
using System.Numerics;

namespace SnakeGame.Mechanics.Collision.Shapes;

internal readonly struct Circle : IFlatShape
{
    public int Polygons { get; init; }
    public Vector2 Position { get; init; }
    public float Radius { get; init; }

    public Polygon AsPolygon()
    {
        var vertexList = new List<Vector2>(Polygons);

        for (int i = 0; i < Polygons; i++)
        {
            float angle = i * 2 * MathF.PI / Polygons;
            float x = Position.X + Radius * MathF.Cos(angle);
            float y = Position.Y + Radius * MathF.Sin(angle);
            vertexList.Add(new Vector2(x, y));
        }

        vertexList.Add(vertexList[0]);
        return Polygon.FromVertexes(vertexList);
    }

    public AABB GetBounds()
    {
        return new AABB()
        {
            Min = Position - new Vector2(Radius, Radius),
            Max = Position + new Vector2(Radius, Radius)
        };
    }
}
