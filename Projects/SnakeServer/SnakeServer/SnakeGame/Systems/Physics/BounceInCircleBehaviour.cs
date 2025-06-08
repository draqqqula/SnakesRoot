using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using System.Numerics;

namespace SnakeGame.Mechanics.Physics;

internal class BounceInCircleBehaviour<T>
    (
    CircleBody Zone, 
    BodyFrame<T> Body,
    ICollisionChecker Collision
    ) : IMovementBehaviour where T : IFlatShape
{

    private const float RotationMuliplier = 0.2f;
    public Vector2 TryMove(Vector2 vector)
    {
        Body.Transform.Position += vector;
        Body.Transform.Angle += vector.Length() * RotationMuliplier;
        if (Vector2.Distance(Zone.Transform.Position, Body.Transform.Position) < Zone.Transform.Size.X / 2)
        {
            return vector;
        }
        return Vector2.Reflect(vector, Vector2.Normalize(Zone.Transform.Position - Body.Transform.Position));
    }
}
