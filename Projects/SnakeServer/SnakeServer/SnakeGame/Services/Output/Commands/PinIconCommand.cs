
using ServerEngine.Models;

namespace SnakeGame.Services.Output.Commands;

internal class PinIconCommand : ISerializableCommand
{
    public List<int> Pinned { get; init; } = [];
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)3);
        writer.Write((byte)Pinned.Count);
        foreach (var id in Pinned)
        {
            writer.Write(id);
        }
    }

    public static void To(ClientIdentifier clientId, CommandSender sender, params int[] pins)
    {
        sender.Send(new PinIconCommand() { Pinned = pins.ToList() }, clientId, 2);
    }
}
