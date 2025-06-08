using SnakeGame.Mechanics.Collision.Shapes;
using System.Numerics;

namespace SnakeGame.Mechanics.Collision.Resolvers;

internal class RSquareToCircleResolver : ICollisionResolver<RotatableSquare, Circle>
{
    public bool IsColliding(RotatableSquare body1, Circle body2)
    {
        if (Vector2.Distance(body1.Position, body2.Position) < body1.DiagonalLength / 2 + body2.Radius)
        {
            return false;
        }
        return Vector2.Distance(body1.TopLeft, body2.Position) <= body2.Radius ||
            Vector2.Distance(body1.TopRight, body2.Position) <= body2.Radius ||
            Vector2.Distance(body1.BottomLeft, body2.Position) <= body2.Radius ||
            Vector2.Distance(body1.BottomRight, body2.Position) <= body2.Radius ||
            IsPointInSquare(body1, body2.Position);
    }
    
    private bool IsPointInSquare(RotatableSquare square, Vector2 point)
    {
        var condition1 = Vector2.Distance(square.TopLeft, point) <= square.Size;
        var condition2 = Vector2.Distance(square.TopRight, point) <= square.Size;
        var condition3 = Vector2.Distance(square.BottomLeft, point) <= square.Size;
        var condition4 = Vector2.Distance(square.BottomRight, point) <= square.Size;

        return Convert.ToByte(condition1) + 
            Convert.ToByte(condition2) + 
            Convert.ToByte(condition3) + 
            Convert.ToByte(condition4) >= 3;
    }
}
