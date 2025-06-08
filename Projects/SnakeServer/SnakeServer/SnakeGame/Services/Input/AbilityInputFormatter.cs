using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using SnakeGame.Models.Input.Internal;

namespace SnakeGame.Services.Input;

internal class AbilityInputFormatter : SignatureInputFormatter<AbilityActivationInput>
{
    public AbilityInputFormatter(IEnumerable<IInputService<AbilityActivationInput>> services) : base(services)
    {
    }

    public override byte Signature => 2;

    public override AbilityActivationInput Deserialize(BinaryReader reader)
    {
        return new AbilityActivationInput()
        {
            Activated = reader.ReadBoolean(),
        };
    }
}
