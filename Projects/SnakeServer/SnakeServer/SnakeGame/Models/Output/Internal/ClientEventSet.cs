using ServerEngine.Models;
using SnakeGame.Mechanics.Frames;

namespace SnakeGame.Models.Output.Internal;

internal readonly struct ClientEventSet
{
    public required ClientIdentifier Id { get; init; }
    public required EventTable Table { get; init; }
}
