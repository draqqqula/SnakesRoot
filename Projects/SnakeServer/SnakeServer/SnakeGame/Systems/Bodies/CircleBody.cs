using SnakeGame.Mechanics.Collision.Shapes;

namespace SnakeGame.Mechanics.Bodies;

internal class CircleBody : BodyFrame<Circle>
{
    public override IEnumerable<Circle> GetBody()
    {
        yield return new Circle()
        {
            Radius = Transform.Size.X,
            Position = Transform.Position,
        };
    }
}
