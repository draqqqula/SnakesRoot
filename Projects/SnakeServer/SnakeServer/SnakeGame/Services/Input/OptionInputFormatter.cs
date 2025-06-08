using ServerEngine.Interfaces.Services;
using SnakeGame.Models.Input.Internal;

namespace SnakeGame.Services.Input;

internal class OptionInputFormatter : SignatureInputFormatter<OptionInput>
{
    public OptionInputFormatter(IEnumerable<IInputService<OptionInput>> services) : base(services)
    {
    }

    public override byte Signature => 4;

    public override OptionInput Deserialize(BinaryReader reader)
    {
        return new OptionInput()
        {
            OptionIndex = reader.ReadInt32()
        };
    }
}
