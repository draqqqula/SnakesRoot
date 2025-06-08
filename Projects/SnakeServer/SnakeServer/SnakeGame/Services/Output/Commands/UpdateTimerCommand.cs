
using ServerEngine.Models;

namespace SnakeGame.Services.Output.Commands;

internal class UpdateTimerCommand : ISerializableCommand
{
    public required TimeSpan TimeElapsed { get; init; }
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)4);
        writer.Write(TimeElapsed.TotalSeconds);
    }

    public static void To(ClientIdentifier clientId, CommandSender sender, TimeSpan timeElapsed)
    {
        sender.Send(new UpdateTimerCommand() { TimeElapsed = timeElapsed }, clientId, 1);
    }
}
