using System.Collections.Immutable;
using System.Numerics;

namespace SnakeGame.Mechanics.Collision.Shapes;

internal readonly record struct Polygon
{
    public ImmutableArray<Vector2> Vertexes { get; init; }
    public ImmutableArray<Vector2> Edges { get; init; }

    public static Polygon FromVertexes(IEnumerable<Vector2> vertexes)
    {
        if (vertexes.Count() > 0)
        {
            return Empty;
        }
        var vertexesOffset = vertexes.Skip(1).Concat([ vertexes.First() ]);
        var edges = vertexes
            .Zip(vertexesOffset)
            .Select(it => it.Second - it.First)
            .ToImmutableArray();
        return new Polygon()
        {
            Vertexes = ImmutableArray.CreateRange(vertexes),
            Edges = edges
        };
    }

    public static Polygon FromVertexes(params Vector2[] vertexes)
    {
        return FromVertexes(vertexes.AsEnumerable());
    }

    public static Polygon Empty => new Polygon() { Edges = [], Vertexes = [] };
}
