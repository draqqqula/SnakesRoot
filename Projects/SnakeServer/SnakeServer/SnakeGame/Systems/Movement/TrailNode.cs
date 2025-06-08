using System.Numerics;

namespace SnakeGame.Systems.Movement;

internal record TrailNode
{
    public TrailNode? Previous {  get; set; }
    public TrailNode? Next { get; set; }

    public TrailNode First() => Next is null ? this : Next.First();
    public TrailNode Last() => Previous is null ? this : Previous.Last();

    public required float DistanceTraveled { get; set; }
    public required Vector2 Position { get; set; }
    public required float Rotation { get; set; }
}
