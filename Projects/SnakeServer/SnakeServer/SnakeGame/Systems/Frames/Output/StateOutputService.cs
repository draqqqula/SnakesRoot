using FlatSharp;
using MessageSchemes;
using ServerEngine.Interfaces.Output;
using SnakeGame.Models.Output.External;

namespace SnakeGame.Mechanics.Frames.Output;

internal class StateOutputService(FrameStorage Storage) : IOutputProvider<StateBasedBinaryOutput>
{
    public StateBasedBinaryOutput Get()
    {
        var message = Storage.GetMessage();
        var size = EventMessage.Serializer.GetMaxSize(message);
        var buffer = new byte[size + 4];
        var lenghtBytes = BitConverter.GetBytes(buffer.Length);
        lenghtBytes.CopyTo(buffer, 0);
        EventMessage.Serializer.Write(new SpanWriter(), buffer.AsSpan(4), message);
        return new StateBasedBinaryOutput()
        {
            Data = buffer,
        };
    }
}
