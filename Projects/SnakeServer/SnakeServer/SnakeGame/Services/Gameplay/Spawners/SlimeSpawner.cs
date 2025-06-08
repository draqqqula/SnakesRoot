using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeCore.MathExtensions;
using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Mechanics.Physics;
using SnakeGame.Models.Gameplay;
using SnakeGame.Systems.GameObjects.Characters;
using System.Numerics;
using System.Security.Claims;

namespace SnakeGame.Services.Gameplay.Spawners;

internal class SlimeSpawner(

    Dictionary<TeamContext, List<Slime>> Slimes, 
    FrameFactory Factory,
    Dictionary<TeamColor, TeamContext> Teams,
    Dictionary<ClientIdentifier, SnakeCharacter> Characters,
    ICollisionChecker Collision,
    IBodyPartFactory BodyPartFactory

    ) : 

    IUpdateService,
    IStartUpService
{
    private const float MinRadius = 40f;
    private const float Deceleration = 2f;
    private Dictionary<Slime, PhysicsMovement> SlimePhysics = [];
    public void Update(IGameContext context)
    {
        CheckPlayers(context);

        foreach (var slime in Slimes.SelectMany(it => it.Value).ToArray())
        {
            if (!Slimes[slime.Team].Contains(slime))
            {
                continue;
            }
            slime.UpdateStatus(context.DeltaTimeF);
            SlimePhysics[slime].Update(context.DeltaTimeF);

            foreach (var character in Characters.Values)
            {
                if (Collision.IsColliding(character.Head, slime))
                {
                    Contact(character, slime);
                }
            }
        }

        foreach (var group in Slimes)
        {
            var subGroups = group.Value.GroupBy(it => it.GroupId);
            foreach (var subGroup in subGroups)
            {
                ApplyAttraction(subGroup.ToList(), context.DeltaTimeF);
            }
            var balance = group.Value
                .Sum(it => it.Value);
            group.Key.Score = balance;
            group.Key.Area.Radius = MinRadius + MathF.Max(0, MathF.Sqrt(balance));
        }
    }

    public void CheckPlayers(IGameContext context)
    {
        foreach (var player in Characters.Values)
        {
            var area = Teams[player.Team].Area;
            if (Vector2.Distance(player.Head.Transform.Position, area.Transform.Position) <= area.Radius)
            {
                player.OnBase = true;
                continue;
            }
            if (player.OnBase)
            {
                StorePoints(context, player);
            }
            player.OnBase = false;
        }
    }

    public void StorePoints(IGameContext context, SnakeCharacter character)
    {
        foreach (var segment in character.Body.Skip(1).ToArray())
        {
            segment.Item.Store(context);
            character.Body.Remove(segment);
        }
    }

    public Slime Spawn(TeamColor team, Transform transform, byte tier)
    {
        var teamContext = Teams[team];
        Slime slime;
        if (teamContext.PowerUps.Contains("Doubler"))
        {
            slime = new DoubledSlime()
            {
                Transform = Factory.Create($"slime{tier}_doubled", transform with { Size = new Vector2(4) }),
                Tier = tier,
                Team = teamContext,
                TeamColor = team
            };
        }
        else
        {
            slime = new Slime()
            {
                Transform = Factory.Create($"slime{tier}", transform),
                Tier = tier,
                Team = teamContext,
                TeamColor = team
            };
        }
        Slimes[teamContext].Add(slime);
        SlimePhysics.Add(slime, new PhysicsMovement(
            new BounceInCircleBehaviour<RotatableSquare>(teamContext.Area, slime, Collision))
        {
            Deceleration = Deceleration
        });
        return slime;
    }

    public void Start(IGameContext context)
    {
        foreach (var team in Teams.Values)
        {
            Slimes.Add(team, []);
        }
    }

    private void Merge(Slime slimeA, Slime slimeB)
    {
        if (slimeA.Tier != slimeB.Tier || slimeA.Stunned || slimeB.Stunned)
        {
            return;
        }
        slimeA.Tier += 1;
        slimeA.UpdateAsset();
        slimeA.Transform.Position = (slimeA.Transform.Position + slimeB.Transform.Position) * 0.5f;
        Dispose(slimeB);
    }

    private void Dispose(Slime slime)
    {
        Slimes[slime.Team].Remove(slime);
        slime.Transform.Dispose();
        SlimePhysics.Remove(slime);
    }

    public void Contact(SnakeCharacter character, Slime slime)
    {
        if (slime.Stunned)
        {
            return;
        }
        var impactAngle = MathEx.VectorToAngle(slime.Transform.Position - character.Head.Transform.Position);
        if (Teams[character.Team] == slime.Team)
        {
            Push(slime, impactAngle);
            slime.Stun(0.3f);
        }
        else if (character.MaxTier >= slime.Tier)
        {
            character.JoinLast(BodyPartFactory.Create(slime.Transform.ReadOnly, slime.Tier, character.Team));
            Dispose(slime);
        }
        else
        {
            var slimeA = Spawn(slime.TeamColor, slime.Transform.ReadOnly, (byte)(slime.Tier - 1));
            slimeA.Stun(0.3f);
            Push(slimeA, impactAngle + MathF.PI / 6);
            var slimeB = Spawn(slime.TeamColor, slime.Transform.ReadOnly, (byte)(slime.Tier - 1));
            slimeB.Stun(0.3f);
            Push(slimeB, impactAngle - MathF.PI / 6);
            Dispose(slime);
        }
    }

    private void Push(Slime slime, float angle)
    {
        SlimePhysics[slime].AddMomentum(
            MathEx.AngleToVector(MathEx.NormalizeAngle(angle, MathF.PI * 2)));
    }

    public void ApplyAttraction(List<Slime> group, float deltaTime)
    {
        foreach (var slime in group.ToArray())
        {
            if (!group.Contains(slime) || slime.Stunned)
            {
                continue;
            }

            var target = group.Except([slime]).Where(it => it.Tier == slime.Tier).MinBy(it => Vector2.Distance(
                it.Transform.Position, slime.Transform.Position));

            if (target is null)
            {
                continue;
            }

            if (Vector2.Distance(
                target.Transform.Position, slime.Transform.Position) < 
                slime.Transform.Size.X * 0.5f + target.Transform.Size.X * 0.5f)
            {
                Merge(slime, target);
                return;
            }
            var direction = Vector2.Normalize(target.Transform.Position - slime.Transform.Position);
            SlimePhysics[slime].AddMomentum(direction * deltaTime * 2);
        }
    }
}
