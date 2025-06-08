using MessageSchemes;
using SnakeGame.Mechanics.Bodies;
using System;
using System.Collections;
using System.Numerics;
using SnakeCore.Collections;
using Row = System.Collections.Generic.KeyValuePair<int, SnakeGame.Mechanics.Frames.EventEntry>;

namespace SnakeGame.Mechanics.Frames;

internal class EventTable
{
    private Dictionary<int, EventEntry> _data = [];

    public int Count => _data.Count;
    public byte[] Serialize()
    {
        var groups = _data.GroupBy(it => it.Value.Lifecycle);

        var createGroup = groups.Where(
            it => it.Key == EventLifecycle.Create ||
            it.Key == EventLifecycle.Renew).SelectMany(it => it);

        var disposeGroup = groups.Where(
            it => it.Key == EventLifecycle.Dispose ||
            it.Key == EventLifecycle.Renew).SelectMany(it => it);

        var updateGroup = groups.FirstOrDefault(it => it.Key == EventLifecycle.Update)?.AsEnumerable();

        var sleepGroup = groups.FirstOrDefault(it => it.Key == EventLifecycle.Sleep)?.AsEnumerable();

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        return stream.ToArray();
    }

    public EventMessage SerializeFlatSharp()
    {
        var created = new List<FrameInfo>();
        var disposed = new List<int>();
        var sleep = new List<int>();
        var positionEvents = new List<PositionEvent>();
        var sizeEvents = new List<SizeEvent>();
        var angleEvents = new List<AngleEvent>();
        var transfromations = new List<(int, string)>();

        foreach (var row in _data)
        {
            if (row.Value.Lifecycle == EventLifecycle.Create || row.Value.Lifecycle == EventLifecycle.Renew)
            {
                created.Add(new FrameInfo()
                {
                    Asset = row.Value.Asset ?? "error",
                    Frame = new Frame()
                    {
                        Id = row.Key,
                        Angle = row.Value.Angle ?? 0f,
                        Position = FrameUtils.SerializeVector2(row.Value.Position),
                        Size = FrameUtils.SerializeVector2(row.Value.Size)
                    }
                });
            }
            else if (row.Value.Lifecycle == EventLifecycle.Dispose || row.Value.Lifecycle == EventLifecycle.Renew)
            {
                disposed.Add(row.Key);
            }
            else if (row.Value.Lifecycle == EventLifecycle.Update || row.Value.Lifecycle == EventLifecycle.Sleep)
            {
                if (row.Value.Lifecycle == EventLifecycle.Sleep)
                {
                    sleep.Add(row.Key);
                }
                if (row.Value.Position.HasValue)
                {
                    positionEvents.Add(new PositionEvent()
                    {
                        Id = row.Key,
                        Position = FrameUtils.SerializeVector2(row.Value.Position)
                    });
                }
                if (row.Value.Size.HasValue)
                {
                    sizeEvents.Add(new SizeEvent()
                    {
                        Id = row.Key,
                        Size = FrameUtils.SerializeVector2(row.Value.Size)
                    });
                }
                if (row.Value.Angle.HasValue)
                {
                    angleEvents.Add(new AngleEvent()
                    {
                        Id = row.Key,
                        Angle = row.Value.Angle.Value
                    });
                }
                if (row.Value.Asset is not null)
                {
                    transfromations.Add((row.Key, row.Value.Asset));
                }
            }
        }

        
        return new EventMessage()
        {
            AngleEvents = GetList(angleEvents),
            SizeEvents = GetList(sizeEvents),
            PositionEvents = GetList(positionEvents),
            Disposed = GetList(disposed),
            Sleep = GetList(sleep),

            Created = GetList(created
            .GroupBy(it => it.Asset)
            .Select(it => new Group()
            {
                Asset = it.Key,
                Frames = it.Select(it => it.Frame).ToList()
            }).ToArray()),

            Transformations = GetList(transfromations
            .GroupBy(it => it.Item2)
            .Select(it => new Transformation()
            {
                NewAsset = it.Key,
                Frames = it.Select(it => it.Item1).ToList()
            })
            .ToArray())
        };
    }
    private static List<T>? GetList<T>(ICollection<T> collection)
    {
        return collection.Count > 0 ? collection.ToList() : null;
    }
    public EventTable Join(EventTable other)
    {
        var selfOuter = other._data
            .ExceptBy(_data.Keys, it => it.Key)
            .Select(it => KeyValuePair.Create(it.Key, it.Value.Clone()));
        var otherOuter = _data
            .ExceptBy(other._data.Keys, it => it.Key)
            .Select(it => KeyValuePair.Create(it.Key, it.Value.Clone()));
        var inner = this._data
            .Join(other._data,
            it => it.Key,
            it => it.Key,
            (older, newer) => KeyValuePair.Create(older.Key, older.Value.Join(newer.Value)))
            .Where(it => it.Value.IsValid);
        return new EventTable()
        {
            _data = inner
            .Concat(selfOuter)
            .Concat(otherOuter)
            .ToDictionary()
        };
    }

    public bool Contains(int id)
    {
        return _data.ContainsKey(id) && _data[id].IsValid;
    }

    public EventEntry this[int id]
    {
        get 
        {
            if (_data.TryGetValue(id, out var value))
            {
                return value;
            }
            var events = new EventEntry();
            _data.Add(id, events);
            return events;
        }
    }

    public (EventTable, EventTable) Split(Func<int, EventEntry, bool> condition)
    {
        var include = new Dictionary<int, EventEntry>();
        var exclude = new Dictionary<int, EventEntry>();
        foreach (var entry in _data)
        {
            if (condition(entry.Key, entry.Value))
            {
                include.Add(entry.Key, entry.Value.Clone());
            }
            else
            {
                exclude.Add(entry.Key, entry.Value.Clone());
            }
        }
        var includeTable = new EventTable()
        {
            _data = include,
        };
        var excludeTable = new EventTable()
        {
            _data = exclude,
        };
        return (includeTable, excludeTable);
    }

    public EventTable Where(Func<int, EventEntry, bool> condition)
    {
        var include = new Dictionary<int, EventEntry>();
        foreach (var entry in _data)
        {
            if (condition(entry.Key, entry.Value.Clone()))
            {
                include.Add(entry.Key, entry.Value.Clone());
            }
        }
        var includeTable = new EventTable()
        {
            _data = include,
        };
        return includeTable;
    }

    public EventTable Select(Action<EventEntry> func)
    {
        return new EventTable()
        {
            _data = _data.Select(it =>
            {
                var cloned = it.Value.Clone();
                func(cloned);
                return KeyValuePair.Create(it.Key, cloned);
            })
            .ToDictionary(),
        };
    }

    public static EventTable FromState(IEnumerable<TransformInfo> state)
    {
        return new EventTable()
        {
            _data = state.ToDictionary(it => it.Id, it =>
            {
                var entry = new EventEntry()
                {
                    Asset = it.Asset,
                    Angle = it.Transform.Angle,
                    Position = it.Transform.Position,
                    Size = it.Transform.Size
                };
                entry.AddAction(EventLifecycle.Create);
                return entry;
            })
        };
    }
}

internal class EventEntry
{
    public EventLifecycle Lifecycle { get; private set; } = EventLifecycle.Cancel;
    public string? Asset { get; set; }
    public Vector2? Position { get; set; }
    public float? Angle { get; set; }
    public Vector2? Size { get; set; }

    public bool IsValid =>
        Lifecycle != EventLifecycle.Cancel &&
        ((
        Lifecycle == EventLifecycle.Update &&
            (Asset is not null ||
            Position.HasValue ||
            Angle.HasValue ||
            Angle.HasValue))
        ||
        ((Lifecycle == EventLifecycle.Create || Lifecycle == EventLifecycle.Renew) &&
            (Asset is not null &&
            Position.HasValue &&
            Angle.HasValue &&
            Size.HasValue))
        ||
        Lifecycle == EventLifecycle.Dispose
        ||
        Lifecycle == EventLifecycle.Sleep
        );

    public EventEntry Join(EventEntry other)
    {
        return new EventEntry()
        {
            Lifecycle = ResolveLifecycle(this.Lifecycle, other.Lifecycle),
            Asset = other.Asset ?? this.Asset,
            Position = other.Position ?? this.Position,
            Angle = other.Angle ?? this.Angle,
            Size = other.Size ?? this.Size,
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is not null && obj is EventEntry other)
        {
            return this.Position == other.Position &&
                this.Angle == other.Angle &&
                this.Size == other.Size &&
                this.Asset == other.Asset &&
                this.Lifecycle == other.Lifecycle;
        }
        return false;
    }

    public void AddAction(EventLifecycle action)
    {
        Lifecycle = ResolveLifecycle(this.Lifecycle, action);
    }

    public EventEntry Clone()
    {
        return new EventEntry()
        {
            Lifecycle = Lifecycle,
            Angle = Angle,
            Asset = Asset,
            Position = Position,
            Size = Size,
        };
    }

    private EventLifecycle ResolveLifecycle(EventLifecycle first, EventLifecycle second)
    {
        if ((first == EventLifecycle.Create || first == EventLifecycle.Renew) 
            && second == EventLifecycle.Dispose)
        {
            return EventLifecycle.Cancel;
        }
        else if (first == EventLifecycle.Dispose && 
            (second == EventLifecycle.Create || second == EventLifecycle.Renew))
        {
            return EventLifecycle.Renew;
        }
        else if (second == EventLifecycle.Update && first != EventLifecycle.Cancel)
        {
            return first;
        }
        else
        {
            return second;
        }
    }
}

internal enum EventLifecycle
{
    Create,
    Update,
    Dispose,
    Cancel,
    Renew,
    Sleep
}