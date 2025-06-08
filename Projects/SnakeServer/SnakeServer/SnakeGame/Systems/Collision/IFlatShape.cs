using SnakeGame.Mechanics.Collision.Shapes;

namespace SnakeGame.Mechanics.Collision;

internal interface IFlatShape
{
    public AABB GetBounds();
    public Polygon AsPolygon();
}
