using ServerEngine.Interfaces;
using SnakeCore.MathExtensions;
using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Mechanics.Physics;
using SnakeGame.Services.Gameplay.Spawners;
using SnakeGame.Systems.GameObjects.PickUps;
using SnakeGame.Systems.Jobs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.Characters;

internal class ScoreSegment : SquareBody, ICarryable
{
    public class BounceJob(ScoreSegment obj) : IBackgroundTask
    {
        public bool Continue(IGameContext context)
        {
            if (obj.Transform.IsDisposed)
            {
                return false;
            }
            if (obj.Size > obj.NormalSize || obj.Transform.Size.X > obj.NormalSize)
            {
                if (obj.Size <= obj.NormalSize + CatchDelta)
                {
                    obj.Size = obj.NormalSize;
                    obj.Transform.Size = new Vector2(obj.Size, obj.Size);
                    return false;
                }
                obj.Size = MathEx.Lerp(obj.Size, obj.NormalSize, obj.ReformFactor, context.DeltaTimeF);
                obj.Transform.Size = new Vector2(obj.Size, obj.Size);
            }
            return true;
        }
    }

    private const float CatchDelta = 0.1f;
    public InteractionResult Interact(IGameContext context, ICarryable previous)
    {
        if (previous is ScoreSegment segmentB)
        {
            if (Tier < segmentB.Tier)
            {
                return InteractionResult.GoDown;
            }
            else if (Tier == segmentB.Tier)
            {
                Bounce(context);
                Tier += 1;
                UpdateAsset();
                return InteractionResult.Merged;
            }
        }
        return InteractionResult.None;
    }

    public required TeamColor Team { get; set; }
    public byte Tier { get; set; } = 0;
    public int Value => (int)Math.Pow(2, Tier + 1);
    private float Size { get; set; } = 4f;
    private float NormalSize { get; set; } = 4f;
    private float ReformFactor { get; set; } = 0.1f;
    private void Bounce(IGameContext context)
    {
        Size = 6;
        context.Using<IJobScheduler>().Start(this, new BounceJob(this));
    }

    public void UpdateAsset()
    {
        Transform.ChangeAsset($"body{Tier}_{Team}");
    }

    public void Detach(IGameContext context)
    {
        context.Using<List<PickupPoints>>().Add(new PickupPoints()
        {
            Transform = context.Using<FrameFactory>().Create($"pickup{Tier}", Transform.ReadOnly),
            Tier = Tier,
        });
        Transform.Dispose();
    }

    public void Store(IGameContext context)
    {
        context.Using<SlimeSpawner>().Spawn(Team, Transform.ReadOnly, Tier);
        Transform.Dispose();
    }

    public ContactResult Contact(IGameContext context, SnakeCharacter snake)
    {
        if (snake.MaxTier > Tier)
        {
            return ContactResult.Consumed;
        }
        else if (snake.MaxTier < Tier)
        {
            return ContactResult.Killed;
        }
        else
        {
            return ContactResult.Consumed & ContactResult.Killed;
        }
    }
}
