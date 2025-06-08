using SnakeGame.Mechanics.Bodies;

namespace SnakeGame.Mechanics.Frames;

internal readonly struct TransformInfo
{
    public required string Asset { get; init; }
    public required int Id { get; init; }
    public required Transform Transform { get; init; }
}
