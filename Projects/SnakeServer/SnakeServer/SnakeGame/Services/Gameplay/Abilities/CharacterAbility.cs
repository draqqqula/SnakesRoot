using SnakeGame.Services.Output.Commands;
using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Timer;

namespace SnakeGame.Services.Gameplay.Abilities;

internal abstract class CharacterAbility
{
    public SnakeCharacter Owner { get; init; }

    public abstract float CooldownDuration { get; }

    public bool OnCooldown {  get; private set; }

    public bool TryActivate(ITimerScheduler scheduler, CommandSender sender)
    {
        if (OnCooldown)
        {
            return false;
        }
        Use(scheduler);
        OnCooldown = true;
        scheduler.SetSeconds(CooldownDuration, () => OnCooldown = false);
        SetAbilityCooldownCommand.To(Owner.ClientId, sender, CooldownDuration);
        return true;
    }

    public virtual void Register()
    {
    }

    protected abstract void Use(ITimerScheduler scheduler);
}
