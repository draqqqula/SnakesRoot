using MessageSchemes;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Frames.Output.Interfaces;
using System.Drawing;
using System.Numerics;

namespace SnakeGame.Mechanics.Frames;

internal class FrameInfo
{
    public required Frame Frame { get; set; }
    public required string Asset {  get; set; }
}

internal class LegacyFrameRepository(FrameRegistry Registry, FrameStorage Storage) : INotificationListener, IMessageProvider
{
    protected virtual EventMessage EventMessage => new EventMessage()
    {
        AngleEvents = GetList(AngleEvents.Values),
        SizeEvents = GetList(SizeEvents.Values),
        PositionEvents = GetList(PositionEvents.Values),
        Disposed = GetList(Disposed),

        Created =  GetList(Created.Values
            .GroupBy(it => it.Asset)
            .Select(it => new Group() 
            { 
                Asset = it.Key, 
                Frames = it.Select(it => it.Frame).ToList() 
            }).ToArray()),

        Transformations = GetList(Transformed
            .GroupBy(it => it.Value)
            .Select(it => new Transformation() 
            { 
                NewAsset = it.Key, 
                Frames = it.Select(it => it.Key).ToList() 
            })
            .ToArray())
    };
    private static List<T>? GetList<T>(ICollection<T> collection)
    {
        return collection.Count > 0 ? collection.ToList() : null;
    }
    protected Dictionary<int, FrameInfo> Created { get; } = [];
    protected Dictionary<int, PositionEvent> PositionEvents { get; } = [];
    protected Dictionary<int, SizeEvent> SizeEvents { get; } = [];
    protected Dictionary<int, AngleEvent> AngleEvents { get; } = [];
    protected HashSet<int> Disposed { get; } = [];
    protected Dictionary<int, string> Transformed { get; } = [];

    public void NotifyPositionChanged(int id, Vector2 position)
    {
        TryNotifyElementChanged(PositionEvents, id, new PositionEvent()
        {
            Id = id,
            Position = Serialize(position)
        }, it => it.Frame.Position = Serialize(position));
    }

    public void NotifySizeChanged(int id, Vector2 size)
    {
        TryNotifyElementChanged(SizeEvents, id, new SizeEvent()
        {
            Id = id,
            Size = Serialize(size)
        }, it => it.Frame.Size = Serialize(size));
    }

    public void NotifyAngleChanged(int id, float angle)
    {
        TryNotifyElementChanged(AngleEvents, id, new AngleEvent()
        {
            Id = id,
            Angle = angle
        }, it => it.Frame.Angle = angle);
    }

    private bool TryNotifyElementChanged<TEvent>(
        Dictionary<int, TEvent> collection, 
        int id, 
        TEvent value, 
        Action<FrameInfo> modifyAction)
    {
        if (Disposed.Contains(id))
        {
            return false;
        }
        if (Created.TryGetValue(id, out var frame))
        {
            modifyAction(frame);
            return true;
        }
        collection[id] = value;
        return true;
    }

    private static bool RemoveIfContains<T>(int id, Dictionary<int, T> pairs)
    {
        if (pairs.ContainsKey(id))
        {
            pairs.Remove(id);
            return true;
        }
        return false;
    }

    public void NotifyTransformed(int id, string newAsset)
    {
        if (TryNotifyElementChanged(
            Transformed, 
            id, 
            newAsset, 
            it => it.Asset = newAsset))
        {
            Storage.ChangeAsset(id, newAsset);
        }
    }

    public void NotifyCreated(int id, string asset, Transform transform)
    {
        Created.Add(id, new FrameInfo()
        {
            Asset = asset,
            Frame = new Frame()
            {
                Angle = transform.Angle,
                Id = id,
                Position = Serialize(transform.Position),
                Size = Serialize(transform.Size)
            }
        });
    }

    public void NotifyDisposed(int id)
    {
        Registry.TryRemove(id);
        Storage.Remove(id);

        if (!RemoveIfContains(id, Created))
        {
            Disposed.Add(id);
        }
        RemoveIfContains(id, PositionEvents);
        RemoveIfContains(id, SizeEvents);
        RemoveIfContains(id, AngleEvents);
        RemoveIfContains(id, Transformed);
    }

    private static Vec2 Serialize(Vector2 vector)
    {
        return new Vec2()
        {
            X = vector.X,
            Y = vector.Y
        };
    }

    public bool TryTakeMessage(out EventMessage? message)
    {
        
        if (
            Disposed.Count != 0 || 
            SizeEvents.Count != 0 || 
            AngleEvents.Count != 0 || 
            PositionEvents.Count != 0 || 
            Created.Count != 0 ||
            Transformed.Count != 0)
        {
            message = EventMessage;
            Created.Clear();
            Disposed.Clear();
            SizeEvents.Clear();
            PositionEvents.Clear();
            AngleEvents.Clear();
            Transformed.Clear();
            return true;
        }
        message = null;
        return false;
    }
}
