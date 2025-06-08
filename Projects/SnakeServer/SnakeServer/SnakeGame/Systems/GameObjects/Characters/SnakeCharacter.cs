using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Models.Input.Internal;
using SnakeGame.Services.Gameplay.Abilities;
using System.Drawing;
using System.Numerics;
using SnakeGame.Systems.Movement;
using System.Text;
using ServerEngine.Models;
using ServerEngine.Interfaces;
using SnakeGame.Systems.Modifiers.Interfaces;
using SnakeGame.Systems.Modifiers;

namespace SnakeGame.Systems.GameObjects.Characters;

internal class SnakeCharacter : SquareBody
{
    public ClientIdentifier ClientId { get; set; }
    public CharacterAbility Ability { get; set; }
    public required List<SnakeSegment> Body { get; init; }
    public required SquareBody Head { get; init; }

    public TeamColor Team { get; set; }

    #region Сharacteristics
    public float MovementDirection { get; set; } = 0f;
    public IModifiable<float> Speed { get; set; } = new ModifiableValue<float>(40f);
    public float BodyIndentation { get; set; } = 4f;
    public IModifiable<float> RotationSpeed { get; set; } = new ModifiableValue<float>(MathF.PI * 2f);
    public IModifiable<float> ExplorationReach { get; set; } = new ModifiableValue<float>(2f);
    public float ShrinkSpeed { get; set; } = 80f;
    public bool OnBase { get; set; } = false;
    public float SortingIntervalMax { get; set; } = 0.5f;
    public float SortingIntervalMin { get; set; } = 0.1f;
    public float SortingIntervalCurrent { get; set; } = 0.5f;
    public float SortingIntervalDecrementStep { get; set; } = 0.05f;

    #endregion

    public byte MaxTier => Body.Select(it => it.Item).OfType<ScoreSegment>().Max(it => it.Tier);

    private float SortingTimer { get; set; } = 0f;
    public bool ActiveSorting { get; set; } = false;

    public void Update(IGameContext context)
    {
        if (!ActiveSorting)
        {
            SortingIntervalCurrent = SortingIntervalMax;
            return;
        }
        if (SortingTimer > SortingIntervalCurrent)
        {
            SortingTimer = 0f;
            SortingIntervalCurrent = MathF.Max(SortingIntervalCurrent - SortingIntervalDecrementStep, SortingIntervalMin);
            ActiveSorting = OneAtATime(context, Body.Count);
            return;
        }
        SortingTimer += context.DeltaTimeF;
    }

    public bool OneAtATime(IGameContext context, int index)
    {
        if (index < 2)
        {
            return false;
        }
        var last = Body[index - 1];
        var nextToLast = Body[index - 2];
        var interaction = nextToLast.Item.Interact(context, last.Item);
        if (interaction == InteractionResult.GoDown)
        {
            Body[index - 1] = nextToLast;
            Body[index - 2] = last;
            var transit = last.Trail.Cut(last.Trail.TotalLength);
            if (transit is not null)
            {
                nextToLast.Trail.ExtendBack(transit);
            }
            return true;
        }
        else if (interaction == InteractionResult.Merged)
        {
            Body.RemoveAt(index - 1);
            last.Item.Transform.Dispose();

            if (last.Trail.Head is not null)
            {
                nextToLast.Trail.ExtendBack(last.Trail.Head);
            }

            return true;
        }
        return OneAtATime(context, index - 1);
    }

    public void JoinLast(ICarryable item)
    {
        var last = Body.LastOrDefault();
        last?.Trail.Clear();
        var transform = last?.Item.Transform.ReadOnly ?? Transform.ReadOnly;
        item.Transform.Position = transform.Position;
        item.Transform.Angle = transform.Angle;
        var newBodypart = new SnakeSegment()
        {
            Item = item
        };
        Body.Add(newBodypart);
        ActiveSorting = true;
    }
}
