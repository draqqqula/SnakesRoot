using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Mechanics.Frames;
using System.Numerics;

namespace SnakeGame.Models.Gameplay;

internal class TeamArea : CircleBody
{
    public float Radius
    {
        get => Transform.Size.X * 0.5f;
        set => Transform.Size = new Vector2(value, value) * 2;
    }
}
