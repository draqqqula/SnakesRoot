using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Output.Commands;

internal class DeclareRuntimeCommandCommand : ISerializableCommand
{
    public required string Name { get; init; }
    public required byte IID { get; init; }
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)11);
        writer.Write(IID);
        writer.Write(Name);
    }
}
