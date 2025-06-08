using SnakeGame.Mechanics.Collision.Shapes;

namespace SnakeGame.Mechanics.Collision;

internal interface IBodyComponent<TShape> where TShape : IFlatShape
{
    public IEnumerable<TShape> GetBody();
}

internal static class BodyExtensions
{
    public static AABB GetLayout<T>(this IBodyComponent<T> body) where T : IFlatShape
    {
        var bounds = body.GetBody().Select(it => it.GetBounds());
        var max = bounds.Max(it => it.Max);
        var min = bounds.Min(it => it.Min);
        return new AABB()
        {
            Max = max,
            Min = min
        };
    }
}
