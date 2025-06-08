
using ServerEngine.Models;

namespace SnakeGame.Services.Output.Commands;

internal class AttachCameraCommand : ISerializableCommand
{
    public int FrameId { get; set; } 
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)1);
        writer.Write(FrameId);
    }

    public static void To(ClientIdentifier clientId, CommandSender sender, int frameId)
    {
        sender.Send(new AttachCameraCommand() { FrameId = frameId }, clientId, 1);
    }
}
