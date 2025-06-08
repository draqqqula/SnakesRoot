using SnakeGame.Mechanics.Collision.Shapes;
using System.Drawing;
using System.Numerics;

namespace SnakeGame.Systems.Collision;

internal static class ShapeOperations
{
    public static AABB Intersect(this AABB self, AABB other)
    {
        var newMin = new Vector2(MathF.Max(self.Min.X, other.Min.X), MathF.Max(self.Min.Y, other.Min.Y));
        var newMax = new Vector2(MathF.Min(self.Max.X, other.Max.X), MathF.Min(self.Max.Y, other.Max.Y));

        if (newMin.X >= newMax.X || newMin.Y >= newMax.Y)
        {
            return new AABB { Min = new Vector2(0, 0), Max = new Vector2(0, 0) };
        }

        return new AABB { Min = newMin, Max = newMax };
    }

    public static IEnumerable<AABB> Subtract(this AABB minuend, AABB subtrahend)
    {
        if (!minuend.Intersects(subtrahend))
        {
            yield return minuend;
            yield break;
        }

        if (minuend.Min.X < subtrahend.Min.X)
        {
            yield return new AABB { Min = minuend.Min, Max = new Vector2(subtrahend.Min.X, minuend.Max.Y) };
        }

        if (minuend.Max.X > subtrahend.Max.X)
        {
            yield return new AABB { Min = new Vector2(subtrahend.Max.X, minuend.Min.Y), Max = minuend.Max };
        }

        if (minuend.Min.Y < subtrahend.Min.Y)
        {
            yield return new AABB { Min = new Vector2(MathF.Max(minuend.Min.X, subtrahend.Min.X), minuend.Min.Y), Max = new Vector2(MathF.Min(minuend.Max.X, subtrahend.Max.X), subtrahend.Min.Y) };
        }

        if (minuend.Max.Y > subtrahend.Max.Y)
        {
            yield return new AABB { Min = new Vector2(MathF.Max(minuend.Min.X, subtrahend.Min.X), subtrahend.Max.Y), Max = new Vector2(MathF.Min(minuend.Max.X, subtrahend.Max.X), minuend.Max.Y) };
        }
    }

    public static bool Intersects(this AABB self, AABB other)
    {
        return self.Min.X < other.Max.X && self.Max.X > other.Min.X && self.Min.Y < other.Max.Y && self.Max.Y > other.Min.Y;
    }
}
