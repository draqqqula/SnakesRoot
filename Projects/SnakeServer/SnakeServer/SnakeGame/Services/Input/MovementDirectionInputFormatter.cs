using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Models.Input.External;
using SnakeGame.Models.Input.Internal;

namespace SnakeGame.Services.Input;

internal class MovementDirectionInputFormatter : SignatureInputFormatter<MovementDirectionInput>
{
    public MovementDirectionInputFormatter(IEnumerable<IInputService<MovementDirectionInput>> services) : base(services)
    {
    }

    public override byte Signature => 3;

    public override MovementDirectionInput Deserialize(BinaryReader reader)
    {
        return new MovementDirectionInput()
        {
            Angle = reader.ReadSingle()
        };
    }
}
