using MessageSchemes;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Frames.Output.Interfaces;
using System.Numerics;

namespace SnakeGame.Mechanics.Frames;

internal class FrameRepository(FrameStorage Storage) : INotificationListener, IMessageProvider, ITableProvider, IUpdateService
{
    protected EventTable Table { get; set; } = new EventTable();

    public void NotifyCreated(int id, string asset, Transform transform)
    {
        var row = Table[id];
        row.AddAction(EventLifecycle.Create);
        row.Asset = asset;
        row.Position = transform.Position;
        row.Size = transform.Size;
        row.Angle = transform.Angle;
    }

    public void NotifyDisposed(int id)
    {
        Storage.Remove(id);
        Table[id].AddAction(EventLifecycle.Dispose);
    }

    public void NotifyAngleChanged(int id, float angle)
    {
        Table[id].AddAction(EventLifecycle.Update);
        Table[id].Angle = angle;
    }

    public void NotifyPositionChanged(int id, Vector2 position)
    {
        Table[id].AddAction(EventLifecycle.Update);
        Table[id].Position = position;
    }

    public void NotifySizeChanged(int id, Vector2 size)
    {
        Table[id].AddAction(EventLifecycle.Update);
        Table[id].Size = size;
    }

    public void NotifyTransformed(int id, string newAsset)
    {
        Table[id].AddAction(EventLifecycle.Update);
        Table[id].Asset = newAsset;
        Storage.ChangeAsset(id, newAsset);
    }

    public bool TryTakeMessage(out EventMessage? message)
    {
        message = Table.SerializeFlatSharp();
        return message.Disposed?.Count > 0 ||
            message.AngleEvents?.Count > 0 ||
            message.SizeEvents?.Count > 0 ||
            message.Created?.Count > 0 ||
            message.Transformations?.Count > 0 ||
            message.PositionEvents?.Count > 0;
    }

    public EventTable Take()
    {
        var saved = Table;
        Table = new EventTable();
        return saved;
    }

    public void Update(IGameContext context)
    {
        Table = new EventTable();
    }
}
