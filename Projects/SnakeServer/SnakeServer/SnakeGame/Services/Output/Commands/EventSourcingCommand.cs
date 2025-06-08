
using FlatSharp;
using MessageSchemes;
using ServerEngine.Models;
using SnakeGame.Mechanics.Frames;

namespace SnakeGame.Services.Output.Commands;

internal class EventSourcingCommand : ISerializableCommand
{
    public required EventTable Table { get; init; }
    public void Serialize(BinaryWriter writer)
    {
        var message = Table.SerializeFlatSharp();
        var size = EventMessage.Serializer.GetMaxSize(message);
        var buffer = new byte[size + 5];
        var lenghtBytes = BitConverter.GetBytes((uint)size);
        lenghtBytes.CopyTo(buffer, 1);
        EventMessage.Serializer.Write(new SpanWriter(), buffer.AsSpan(5), message);
        writer.Write(buffer);
    }

    public static void To(ClientIdentifier clientId, CommandSender sender, EventTable table)
    {
        sender.Send(new EventSourcingCommand() { Table = table }, clientId, 0);
    }
}
