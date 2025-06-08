using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Output.Commands;

internal class ExecuteRuntimeCommandCommand : ISerializableCommand
{
    public required byte IID { get; set; }
    public required ISerializableCommand RuntimeCommand { get; set; }
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)12);
        writer.Write(IID);
        RuntimeCommand.Serialize(writer);
    }
}
