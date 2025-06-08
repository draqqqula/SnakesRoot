using SnakeCore.MathExtensions;
using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Physics;
using SnakeGame.Systems.GameObjects.PickUps;
using System.Numerics;

namespace SnakeGame.Models.Gameplay;

internal class Slime : PickupPoints
{
    private float CatchDistance = 0.01f;
    public void UpdateStatus(float deltaTime)
    {
        if (Math.Abs(DestinedSize - Transform.Size.X) > CatchDistance)
        {
            Transform.Size = new Vector2(MathEx.Lerp(Transform.Size.X, DestinedSize, 0.1f, deltaTime));
        }
        else if (DestinedSize - Transform.Size.X > 0)
        {
            Transform.Size = new Vector2(DestinedSize);
        }
        StunDuration = MathF.Max(0, StunDuration - deltaTime);
    }
    public void Stun(float duration)
    {
        StunDuration = duration;
    }
    public virtual byte GroupId => 0;
    public float StunDuration { get; private set; }
    public bool Stunned => StunDuration > 0;
    public required TeamContext Team { get; init; }
    public required TeamColor TeamColor { get; init; }
    public virtual float DestinedSize => 4 + Tier;
    public virtual void UpdateAsset()
    {
        Transform.ChangeAsset($"slime{Tier}");
    }
}
