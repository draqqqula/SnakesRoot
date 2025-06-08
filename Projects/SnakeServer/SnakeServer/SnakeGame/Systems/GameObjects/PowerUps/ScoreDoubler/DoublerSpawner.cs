using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using SnakeGame.Common;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Models.Gameplay;
using SnakeGame.Systems.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.PowerUps.ScoreDoubler;

internal class DoublerSpawner(Dictionary<TeamColor, TeamContext> Teams, PowerUpSpawner Spawner, FrameFactory FrameFactory) : IStartUpService
{
    private bool AlreadySpawned { get; set; } = false;
    public void Start(IGameContext context)
    {
        foreach (var team in Teams)
        {
            team.Value.ScoreChangedEvent += TrySpawn;
        }
    }

    private void TrySpawn(int score)
    {
        if (AlreadySpawned || score < 8)
        {
            return;
        }
        var transform = new Mechanics.Bodies.Transform() { Angle = 0, Position = Vector2.Zero, Size = new Vector2(6) };
        var powerUp = new ScoreDoubler()
        {
            Transform = FrameFactory.Create($"DoublerPowerUp_Inactive", transform)
        };
        Spawner.Setup(powerUp, 15);
        AlreadySpawned = true;
    }
}
