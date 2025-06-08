using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Systems.Bodies;

namespace SnakeGame.Mechanics.Bodies;

internal abstract class BodyFrame<TShape> : ITransformable, IBodyComponent<TShape> where TShape : IFlatShape
{
    public required TransformBase Transform { get; init; }
    public abstract IEnumerable<TShape> GetBody();
}