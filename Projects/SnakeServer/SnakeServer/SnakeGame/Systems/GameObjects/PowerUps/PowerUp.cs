using ServerEngine.Interfaces;
using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Models.Gameplay;
using SnakeGame.Systems.GameObjects.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.PowerUps;

internal abstract class PowerUp : SquareBody, ICarryable
{
    public ItemState State { get; private set; } = ItemState.Inactive;
    public TeamColor? Claim { get; private set; }
    public abstract string Name { get; }

    private void ChangeState(ItemState state)
    {
        State = state;
        Transform.ChangeAsset($"{Name}PowerUp_{state}");
    }

    public void Activate()
    {
        ChangeState(ItemState.Pickup);
        Claim = null;
    }

    public void Pickup(SnakeCharacter snake)
    {
        Transform.Size = new System.Numerics.Vector2(4);
        snake.JoinLast(this);
        Claim = snake.Team;
        ChangeState(ItemState.Carried);
    }

    public InteractionResult Interact(IGameContext context, ICarryable other)
    {
        if (other is PowerUp powerUp)
        {
            return InteractionResult.None;
        }
        return InteractionResult.GoDown;
    }

    public ContactResult Contact(IGameContext context, SnakeCharacter snake)
    {
        return ContactResult.Consumed;
    }

    public void Detach(IGameContext context)
    {
        ChangeState(ItemState.Pickup);
    }

    public void Store(IGameContext context)
    {
        Transform.Size = new System.Numerics.Vector2(6);
        ChangeState(ItemState.Claimed);
        if (Claim.HasValue && context.Using<Dictionary<TeamColor, TeamContext>>().TryGetValue(Claim.Value, out var teamContext))
        {
            teamContext.PowerUps.Add(Name);
        }
    }
}
