using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using System.Numerics;

namespace SnakeGame.Mechanics.Physics;

internal class PhysicsMovement(IMovementBehaviour behaviour)
{
    public IMovementBehaviour Behaviour { get; set; } = behaviour;
    public float Deceleration { get; set; } = 0f;
    public Vector2 ResultingVector { get; private set; } = Vector2.Zero;
    public void AddMomentum(Vector2 vector)
    {
        ResultingVector += vector;
    }

    public void Update(float deltaTime) 
    {
        var newVector = Behaviour.TryMove(ResultingVector);
        if (newVector == Vector2.Zero)
        {
            ResultingVector = Vector2.Zero;
            return;
        }
        var decelerationComponent = Vector2.Normalize(newVector) * Deceleration * deltaTime;
        if (decelerationComponent.Length() >= newVector.Length())
        {
            ResultingVector = Vector2.Zero;
            return;
        }
        ResultingVector = newVector - decelerationComponent;
    }
}
