using MessageSchemes;
using System.Numerics;

namespace SnakeGame.Mechanics.Frames;

internal static class FrameUtils
{
    public static Vec2 SerializeVector2(Vector2? vector)
    {
        if (vector.HasValue)
        {
            return new Vec2()
            {
                X = vector.Value.X,
                Y = vector.Value.Y
            };
        }
        return new Vec2();
    }
}
