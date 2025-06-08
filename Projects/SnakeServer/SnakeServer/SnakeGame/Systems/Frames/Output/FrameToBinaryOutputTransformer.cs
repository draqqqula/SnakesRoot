using FlatSharp;
using MessageSchemes;
using ServerEngine.Interfaces.Output;
using SnakeGame.Models.Output.External;

namespace SnakeGame.Mechanics.Frames.Output;

internal class FrameToBinaryOutputTransformer : IOutputCollector<EventMessage>, IOutputProvider<EventBasedBinaryOutput?>
{
    EventBasedBinaryOutput? Output { get; set; }
    public EventBasedBinaryOutput? Get()
    {
        return Output;
    }

    public void Pass(EventMessage data)
    {
        var size = EventMessage.Serializer.GetMaxSize(data);
        var buffer = new byte[size + 4];
        var lenghtBytes = BitConverter.GetBytes(buffer.Length);
        lenghtBytes.CopyTo(buffer, 0);
        EventMessage.Serializer.Write(new SpanWriter(), buffer.AsSpan(4), data);
        Output = new EventBasedBinaryOutput()
        {
            EventData = buffer
        };
    }
}
