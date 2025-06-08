using SnakeGame.Services.Gameplay.Abilities;
using SnakeGame.Systems.GameObjects.Characters;

namespace SnakeGame.Systems.Respawn;

internal interface IAbilityFactory
{
    public CharacterAbility Create(SnakeCharacter character);
}
