using ServerEngine.Interfaces;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Systems.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.Characters;

internal interface ICarryable : IBodyComponent<RotatableSquare>, ITransformable
{
    public InteractionResult Interact(IGameContext context, ICarryable other);
    public ContactResult Contact(IGameContext context, SnakeCharacter snake);
    public void Detach(IGameContext context);
    public void Store(IGameContext context);
}
