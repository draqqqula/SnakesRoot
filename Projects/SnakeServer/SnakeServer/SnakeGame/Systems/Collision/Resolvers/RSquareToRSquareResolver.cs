using SnakeGame.Mechanics.Collision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Mechanics.Collision.Resolvers
{
    internal class RSquareToRSquareResolver
        (ICollisionResolver<Polygon, Polygon> PolygonResolver,
        ICollisionResolver<AABB, AABB> AABBResolver)
        : ICollisionResolver<RotatableSquare, RotatableSquare>
    {
        public bool IsColliding(RotatableSquare square1, RotatableSquare square2)
        {
            var distance = Vector2.Distance(square1.Position, square2.Position);

            if (distance > square1.DiagonalLength/2 + square2.DiagonalLength/2)
            {
                return false;
            }

            if (distance <= square1.Size/2 + square2.Size/2)
            {
                return true;
            }
            if (square1.Rotation % (MathF.PI/2) == 0 && square2.Rotation % (MathF.PI / 2) == 0)
            {
                AABBResolver.IsColliding(square1.GetUnrotated(), square2.GetUnrotated());
            }
            return PolygonResolver.IsColliding(square1.AsPolygon(), square2.AsPolygon());
        }
    }
}
