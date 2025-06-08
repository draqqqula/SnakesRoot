using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using System.Numerics;

namespace SnakeGame.Mechanics.Physics;

internal interface IMovementBehaviour
{
    public Vector2 TryMove(Vector2 vector);
}
