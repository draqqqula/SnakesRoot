using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeCore.MathExtensions;
using SnakeCore.MathExtensions.Hexagons;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Respawn;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeGame.Systems.Digging;

internal class DiggingManager(

    Terrain Terrain, 
    Dictionary<ClientIdentifier, SnakeCharacter> Snakes, 
    TerrainDataSender DataSender)

    : IUpdateService, IEventListener<RespawnEventArgs>
{
    private const float SpeedSlowdownShare = 0.8f;
    private const float RotationSpeedSlowdownShare = 0.6f;
    private const float ExplorationReachIncreaceShare = -0.5f;
    private const float DiggerOffset = 2f;

    private Dictionary<ClientIdentifier, SlowdownRateProvider> _slowdownRateProviders = [];

    public void OnFired(RespawnEventArgs arguments)
    {
        var snake = arguments.Character;
        var provider = new SlowdownRateProvider();
        _slowdownRateProviders[snake.ClientId] = provider;
        var speedModifier = new DiggingSpeedModifier(snake.Speed, provider, SpeedSlowdownShare);
        var rotationSpeedModifier = new DiggingSpeedModifier(snake.RotationSpeed, provider, RotationSpeedSlowdownShare);
        var explorationModifier = new DiggingSpeedModifier(snake.ExplorationReach, provider, ExplorationReachIncreaceShare);
        snake.Speed = speedModifier;
        snake.RotationSpeed = rotationSpeedModifier;
        snake.ExplorationReach = explorationModifier;
        speedModifier.Enabled = true;
        rotationSpeedModifier.Enabled = true;
        explorationModifier.Enabled = true;
    }

    public void Update(IGameContext context)
    {
        var broken = UpdateMap(context.DeltaTimeF).ToArray();
        DataSender.UpdateForClients(broken);
    }

    private IEnumerable<HexagonTile> UpdateMap(float deltaTime)
    {
        foreach (var snake in Snakes)
        {
            _slowdownRateProviders[snake.Key].Decreace(deltaTime);

            var direction = MathEx.AngleToVector(snake.Value.Transform.Angle);
            var position = snake.Value.Transform.Position + direction * DiggerOffset;
            var explorationZone = new Circle()
            {
                Position = position,
                Radius = snake.Value.ExplorationReach.Value
            };

            var diggingZone = new Circle()
            {
                Position = position,
                Radius = 3
            };

            var tiles = Terrain
                .Dig(explorationZone, diggingZone)
                .ToArray();

            if (tiles.Length > 0)
            {
                _slowdownRateProviders[snake.Key].Increace(tiles.Length);
            }

            foreach (var tile in tiles)
            {
                yield return tile;
            }
        }
    }
}
