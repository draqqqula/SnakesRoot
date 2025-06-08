using MessageSchemes;

namespace SnakeGame.Mechanics.Frames.Output.Interfaces;

internal interface IMessageProvider
{
    public bool TryTakeMessage(out EventMessage? message);
}
