
using ServerEngine.Models;

namespace SnakeGame.Services.Output.Commands;

internal class SetAbilityCooldownCommand : ISerializableCommand
{
    public float Duration { get; set; }
    public void Serialize(BinaryWriter writer)
    {
        writer.Write((byte)7);
        writer.Write(Duration);
    }

    public static void To(ClientIdentifier clientId, CommandSender sender, float duration)
    {
        sender.Send(new SetAbilityCooldownCommand()
        {
            Duration = duration
        }, clientId, 1);
    }
}
