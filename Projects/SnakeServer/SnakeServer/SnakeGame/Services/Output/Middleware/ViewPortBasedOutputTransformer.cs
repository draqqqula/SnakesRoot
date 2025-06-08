using FlatSharp;
using MessageSchemes;
using ServerEngine.Interfaces.Output;
using ServerEngine.Models;
using SnakeGame.Mechanics.ViewPort;
using SnakeGame.Models.Output.External;
using SnakeGame.Models.Output.Internal;
using ClientCommandCollection =
    System.Collections.Generic.Dictionary<
        ServerEngine.Models.ClientIdentifier,
        System.Collections.Generic.List<SnakeGame.Services.Output.ISerializableCommand>>;

namespace SnakeGame.Services.Output.Services;

internal class ViewPortBasedOutputTransformer :
    IOutputCollector<ClientCommandWrapper>, IOutputProvider<ViewPortBasedBinaryOutput>
{
    private readonly ClientCommandCollection _data = [];
    public ViewPortBasedBinaryOutput Get()
    {
        return new ViewPortBasedBinaryOutput(
            _data.ToDictionary(it => it.Key, it => Serialize(it.Value)));
    }

    public void Pass(ClientCommandWrapper data)
    {
        _data.TryAdd(data.Id, []);
        _data[data.Id].Add(data.Command);
    }

    private byte[] Serialize(IEnumerable<ISerializableCommand> commands)
    {
        var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        foreach (var command in commands)
        {
            command.Serialize(writer);
        }
        writer.Flush();
        return stream.ToArray();
    }
}
