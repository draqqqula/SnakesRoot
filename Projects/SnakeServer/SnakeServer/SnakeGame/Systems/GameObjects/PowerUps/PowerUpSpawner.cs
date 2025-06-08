using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Services;
using SnakeGame.Services.Gameplay;
using SnakeGame.Services.Output.Commands;
using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Service;
using SnakeGame.Systems.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.PowerUps;

internal class PowerUpSpawner
    (

    IClientRegistry Registry,
    List<PowerUp> PowerUps,
    CommandSender Sender,
    FrameFactory FrameFactory,
    ICollisionChecker CollisionChecker,
    Dictionary<ClientIdentifier, SnakeCharacter> Characters,
    ITimerScheduler Scheduler,
    MinimapManager MinimapManager

    ) :

    IUpdateService
{
    public void Update(IGameContext context)
    {
        foreach (PowerUp powerUp in PowerUps)
        {
            if (powerUp.State == ItemState.Pickup)
            {
                foreach (var snake in Characters.Values)
                {
                    if (CollisionChecker.IsColliding(snake.Head, powerUp))
                    {
                        powerUp.Pickup(snake);
                    }
                }
            }
        }
    }

    public void Setup(PowerUp powerUp, int delay)
    {
        PowerUps.Add(powerUp);
        Broadcast(powerUp, delay);
        Scheduler.SetSeconds(delay, powerUp.Activate);
    }

    private void Broadcast(PowerUp powerUp, int activationTime)
    {
        var notify = new NotifyPowerUpCommand()
        {
            ActivationTime = activationTime,
            Name = powerUp.Name
        };
        foreach (var id in Registry.Online)
        {
            MinimapManager.Pin(id, Sender, powerUp.Transform.Id.Value);
            Sender.Send(notify, id, 0);
        }
    }
}
