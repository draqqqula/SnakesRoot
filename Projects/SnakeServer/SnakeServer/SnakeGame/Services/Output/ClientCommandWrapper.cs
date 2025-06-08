using ServerEngine.Models;

namespace SnakeGame.Services.Output;

internal readonly struct ClientCommandWrapper
{
    public ClientIdentifier Id { get; init; }
    public ISerializableCommand Command { get; init; }
}
