using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeCore.MathExtensions;
using SnakeGame.Systems.GameObjects.Characters;
using System.Drawing;
using System.Numerics;

namespace SnakeGame.Systems.Movement;

internal class TrailMovementManager(Dictionary<ClientIdentifier, SnakeCharacter> Characters) : IUpdateService
{
    private const float HeadOffset = 4f;
    public RectangleF PlayableZone = new RectangleF(-250, -250, 500, 500);

    public void Update(IGameContext context)
    {
        foreach (var character in Characters.Values)
        {
            character.Transform.Angle = character.Transform.Angle.RotateTowards(
                character.MovementDirection,
                MathF.PI * 2,
                character.RotationSpeed.Value,
                context.DeltaTimeF);

            var direction = MathEx.AngleToVector(character.Transform.Angle);
            var distance = character.Speed.Value * direction * context.DeltaTimeF;

            if (!PlayableZone.Contains((PointF)(character.Transform.Position + distance)))
            {
                character.MovementDirection = 
                    MathEx.VectorToAngle((Vector2)(PlayableZone.Location + PlayableZone.Size / 2) - character.Transform.Position);
            }

            character.Transform.Position += distance;
            character.Head.Transform.Position = character.Transform.Position + direction * HeadOffset;
            character.Head.Transform.Angle = character.Transform.Angle;

            if (distance == Vector2.Zero)
            {
                return;
            }

            var transit = new TrailNode()
            {
                DistanceTraveled = distance.Length(),
                Position = character.Transform.Position,
                Rotation = character.Transform.Angle
            };

            foreach (var part in character.Body)
            {
                if (transit is not null)
                {
                    part.Item.Transform.Position = transit.First().Position;
                    part.Item.Transform.Angle = transit.First().Rotation;

                    part.Trail.ExtendFront(transit);
                    transit = null;
                }
                var extra = Math.Min(part.Trail.TotalLength - character.BodyIndentation, character.ShrinkSpeed * context.DeltaTimeF);
                if (extra > 0)
                {
                    transit = part.Trail.Cut(extra);
                }
            }
        }
    }
}
