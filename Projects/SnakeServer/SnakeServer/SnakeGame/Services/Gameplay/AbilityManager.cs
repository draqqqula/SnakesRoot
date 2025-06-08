using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Models.Input.Internal;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Killfeed;
using SnakeGame.Systems.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Gameplay;

internal class AbilityManager(

    Dictionary<ClientIdentifier, SnakeCharacter> Characters,
    ITimerScheduler Scheduler,
    CommandSender Sender

    ) : IInputService<AbilityActivationInput>
{
    public event Action<KillInteraction> Fire;

    public void OnInput(ClientIdentifier id, AbilityActivationInput data)
    {
        if (Characters.TryGetValue(id, out var character))
        {
            if (data.Activated)
            {
                character.Ability.TryActivate(Scheduler, Sender);
            }
        }
    }
}
