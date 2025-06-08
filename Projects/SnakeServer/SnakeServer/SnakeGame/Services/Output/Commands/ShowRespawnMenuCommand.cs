
using ServerEngine.Models;

namespace SnakeGame.Services.Output.Commands;

internal class ShowRespawnMenuCommand : ISerializableCommand
{
    public required float RespawnTimer { get; init; }
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)6);
        writer.Write(RespawnTimer);
    }

    public static void To(ClientIdentifier clientId, CommandSender sender, TimeSpan time)
    {
        sender.Send(new ShowRespawnMenuCommand()
        {
            RespawnTimer = (float)time.TotalSeconds
        }, clientId, 2);
    }
}
