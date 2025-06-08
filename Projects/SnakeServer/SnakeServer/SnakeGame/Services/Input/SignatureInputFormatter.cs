using ServerEngine;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Models.Input.External;
using SnakeGame.Models.Input.Internal;

namespace SnakeGame.Services.Input;

internal abstract class SignatureInputFormatter<T>(IEnumerable<IInputService<T>> services) : IInputFormatter<BinaryInput>
{
    public abstract byte Signature { get; }
    public ResolveResult TryResolve(BinaryInput input, ClientIdentifier id)
    {
        if (input.Data.BaseStream.Position == input.Data.BaseStream.Length)
        {
            return ResolveResult.Error;
        }

        var openingByte = input.Data.ReadByte();

        if (openingByte == Signature)
        {
            var model = Deserialize(input.Data);
            foreach (var service in services)
            {
                service.OnInput(id, model);
            }

            if (input.Data.BaseStream.Position == input.Data.BaseStream.Length)
            {
                return ResolveResult.Success;
            }
            else
            {
                return ResolveResult.Proceed;
            }
        }
        input.Data.BaseStream.Position -= 1;
        return ResolveResult.Failure;
    }

    public abstract T Deserialize(BinaryReader reader);
}
