using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Modifiers;
using SnakeGame.Systems.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Gameplay.Abilities.Dash;

internal class DashAbility : CharacterAbility
{
    protected DashSpeedModifier _modifier;
    public override float CooldownDuration => 5f;

    protected override void Use(ITimerScheduler scheduler)
    {
        _modifier.Enabled = true;
        Owner.BodyIndentation = 8f;
        scheduler.SetSeconds(0.2f, () => CancelDash(Owner));
    }

    private void CancelDash(SnakeCharacter character)
    {
        character.BodyIndentation = 4f;
        _modifier.Enabled = false;
    }

    public override void Register()
    {
        var modifier = new DashSpeedModifier(Owner.Speed);
        Owner.Speed = modifier;
        _modifier = modifier;
    }
}
