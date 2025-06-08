using SnakeGame.Mechanics.Collision.Shapes;

namespace SnakeGame.Mechanics.Bodies;

internal class RectangleBody : BodyFrame<AABB>
{
    public override IEnumerable<AABB> GetBody()
    {
        yield return new AABB()
        {
            Min = Transform.Position - Transform.Size * 0.5f,
            Max = Transform.Position + Transform.Size * 0.5f
        };
    }
}
