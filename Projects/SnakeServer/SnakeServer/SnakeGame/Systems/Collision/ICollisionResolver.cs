namespace SnakeGame.Mechanics.Collision;

internal interface ICollisionResolver<T1,T2>
{
    public bool IsColliding(T1 body1, T2 body2);
}
