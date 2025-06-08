using System.Numerics;

namespace SnakeGame.Models.Input.Internal;

public enum MovementDirection
{
    Left,
    Right,
    Up,
    Down
}
public struct MovementDirectionInput
{
    public required float Angle { get; init; }
}

public static class MovementDirectionExtensions
{
    public static Vector2 ToVector2(this MovementDirection direction)
    {
        switch (direction)
        {
            case MovementDirection.Left: return new Vector2(-1, 0);
            case MovementDirection.Right: return new Vector2(1, 0);
            case MovementDirection.Up: return new Vector2(0, 1);
            case MovementDirection.Down: return new Vector2(0, -1);
            default: return Vector2.Zero;
        }
    }

    public static float ToDirection(this MovementDirection direction)
    {
        switch (direction)
        {
            case MovementDirection.Left: return MathF.PI;
            case MovementDirection.Right: return 0;
            case MovementDirection.Up: return MathF.PI * 0.5f;
            case MovementDirection.Down: return MathF.PI * 1.5f;
            default: return MathF.PI;
        }
    }

    public static MovementDirection ToDirection(this byte key)
    {
        switch (key)
        {
            case 1: return MovementDirection.Left;
            case 2: return MovementDirection.Right;
            case 3: return MovementDirection.Down;
            case 4: return MovementDirection.Up;
            default: return MovementDirection.Left;
        }
    }
}