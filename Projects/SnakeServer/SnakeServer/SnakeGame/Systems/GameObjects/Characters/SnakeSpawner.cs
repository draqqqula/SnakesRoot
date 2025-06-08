using MessageSchemes;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Output;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeCore.MathExtensions;
using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision.Resolvers;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Mechanics.ViewPort;
using SnakeGame.Models.Gameplay;
using SnakeGame.Models.Input.Internal;
using SnakeGame.Models.Output.Internal;
using SnakeGame.Systems.GameObjects.PickUps;
using SnakeGame.Systems.Respawn;
using System.Numerics;

namespace SnakeGame.Systems.GameObjects.Characters;

internal class SnakeSpawner(
    Dictionary<ClientIdentifier, SnakeCharacter> Players,
    List<PickupPoints> Pickups,
    Dictionary<TeamColor, TeamContext> Teams,
    FrameFactory FrameFactory,
    IBodyPartFactory BodyPartFactory
    ) :
    ISessionService, IInputService<MovementDirectionInput>, IUpdateService
{
    private Random SpawnPositionRandom { get; } = new Random();
    public void OnInput(ClientIdentifier id, MovementDirectionInput data)
    {
        if (Players.TryGetValue(id, out SnakeCharacter character))
        {
            character.MovementDirection = data.Angle;
        }
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
    }

    public SnakeCharacter Spawn(ClientIdentifier id, IAbilityFactory abilityFactory)
    {
        var team = Teams.Where(it => it.Value.Members.Contains(id)).First();
        var position =
            team.Value.Area.Transform.Position +
            MathEx.AngleToVector(SpawnPositionRandom.NextSingle() * MathF.PI * 2) *
            (team.Value.Area.Radius / 2);
        var angle = MathEx.AngleBetweenVectors(position, Vector2.Zero);

        var transform = new Transform()
        {
            Angle = angle,
            Position = position,
            Size = new Vector2(4, 4),
        };

        var main = new TransformObject()
        {
            Angle = transform.Angle,
            Position = position,
            Size = transform.Size,
        };
        var head = new SquareBody()
        {
            Transform = FrameFactory.Create($"head_{team.Key}", transform)
        };
        var body = new SnakeSegment()
        {
            Item = BodyPartFactory.Create(transform, 0, team.Key)
        };

        var character = new SnakeCharacter()
        {
            Transform = main,
            Head = head,
            Body = [body],
            MovementDirection = angle,
            Team = team.Key,
            ClientId = id
        };
        character.Ability = abilityFactory.Create(character);
        Players.TryAdd(id, character);
        return character;
    }

    public void Despawn(IGameContext context, ClientIdentifier id)
    {
        var snake = Players[id];
        foreach (var segment in snake.Body)
        {
            segment.Item.Detach(context);
        }
        Players.Remove(id);
        snake.Head.Transform.Dispose();
        snake.Transform.Dispose();
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        Despawn(context, id);
    }

    public void Update(IGameContext context)
    {
        foreach (var character in Players.Values)
        {
            character.Update(context);
        }
    }
}