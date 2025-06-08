using SnakeGame.Mechanics.Frames;

namespace SnakeGame.Models.Output.External;

public struct EventBasedBinaryOutput
{
    public required byte[] EventData { get; init; }
}
