using SnakeGame.Systems.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Gameplay.Abilities.Magnet;

internal class MagnetAbility : CharacterAbility
{
    public override float CooldownDuration => 4f;

    protected override void Use(ITimerScheduler scheduler)
    {
        throw new NotImplementedException();
    }
}
