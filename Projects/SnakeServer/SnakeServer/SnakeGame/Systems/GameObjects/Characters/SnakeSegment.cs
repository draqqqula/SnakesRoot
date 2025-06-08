using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Systems.Movement;
using System.Numerics;

namespace SnakeGame.Systems.GameObjects.Characters;

internal class SnakeSegment
{
    public required ICarryable Item { get; set; }
    public Trail Trail { get; set; } = new Trail();
}
