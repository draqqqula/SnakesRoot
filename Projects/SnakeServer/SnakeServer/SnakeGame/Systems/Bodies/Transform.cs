using System.Numerics;

namespace SnakeGame.Mechanics.Bodies;

internal struct Transform
{
    public required Vector2 Position { get; init; }
    public required float Angle { get; init; }
    public required Vector2 Size { get; init; }
}
