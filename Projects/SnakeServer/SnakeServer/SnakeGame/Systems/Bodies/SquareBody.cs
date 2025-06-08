using SnakeGame.Mechanics.Collision.Shapes;

namespace SnakeGame.Mechanics.Bodies;

internal class SquareBody : BodyFrame<RotatableSquare>
{
    public override IEnumerable<RotatableSquare> GetBody()
    {
        yield return new RotatableSquare()
        {
            Rotation = Transform.Angle,
            Position = Transform.Position,
            Size = Transform.Size.X
        };
    }
}
