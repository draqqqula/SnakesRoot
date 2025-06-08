using ServerEngine.Interfaces.Serialization;
using SnakeGame.Services.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.RuntimeCommands;

internal struct PrefixedCommand<T>(IBinarySerializer<T> Serializer) : ISerializableCommand
{
    public required T Value { get; init; }
    public required ISerializableCommand? Command {  get; init; }

    public void Serialize(BinaryWriter writer)
    {
        if (Command is not null)
        {
            Command.Serialize(writer);
        }
        Serializer.Serialize(writer, Value);
    }
}
