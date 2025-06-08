using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using System.Drawing;
using System.Numerics;

namespace SnakeGame.Systems.GameObjects.PickUps;

internal class PickupPoints : SquareBody
{
    public required byte Tier { get; set; }
    public virtual int Value => (int)Math.Pow(2, Tier + 1);
}
