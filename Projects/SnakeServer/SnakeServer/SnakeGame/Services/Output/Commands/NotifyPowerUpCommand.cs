using ServerEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Output.Commands;

internal class NotifyPowerUpCommand : ISerializableCommand
{
    public required string Name { get; init; }
    public required int ActivationTime { get; init; }
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)9);
        writer.Write(ActivationTime);
        writer.Write(Name);
    }
}
