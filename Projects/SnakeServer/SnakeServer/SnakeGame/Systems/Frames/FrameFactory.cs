using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Frames.Output.Interfaces;

namespace SnakeGame.Mechanics.Frames;

internal class FrameFactory(INotificationListener Listener, FrameRegistry Registry, FrameStorage storage)
{
    public TransformFrame Create(string asset, Transform transform)
    {
        var id = Registry.GetKey();
        var frame = new TransformFrame(Listener, id, transform);
        storage.Add(id, asset, frame);
        Listener.NotifyCreated(id, asset, transform);
        return frame;
    }
}
