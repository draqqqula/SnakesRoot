using MessageSchemes;

namespace SnakeGame.Mechanics.Frames;

internal class FrameStorage
{
    private readonly Dictionary<int, string> _idToAssetMatcher = new Dictionary<int, string>();
    private readonly Dictionary<string, Dictionary<int, TransformFrame>> _frames = [];
    public void Add(int id, string asset, TransformFrame frame)
    {
        _idToAssetMatcher.Add(id, asset);
        if (!_frames.ContainsKey(asset))
        {
            _frames.Add(asset, []);
        }
        _frames[asset].Add(id, frame);
    }

    public void Remove(int id)
    {
        if (_idToAssetMatcher.TryGetValue(id, out var asset))
        {
            if (_frames.TryGetValue(asset, out var repos))
            {
                repos.Remove(id);
            }
        }
        _idToAssetMatcher.Remove(id);
    }

    public IEnumerable<TransformInfo> GetAll()
    {
        return _frames.SelectMany(group => group.Value.Select(it => new TransformInfo()
        {
            Asset = group.Key,
            Id = it.Key,
            Transform = it.Value.ReadOnly
        }));
    }

    public void ChangeAsset(int id, string newAsset)
    {
        if (_idToAssetMatcher.TryGetValue(id, out var oldAsset))
        {
            _idToAssetMatcher.Remove(id);
            var frame = _frames[oldAsset][id];
            _frames[oldAsset].Remove(id);

            Add(id, newAsset, frame);
        }
    }
    
    public EventMessage GetMessage()
    {
        var frames = _frames.Select(it => new Group() 
        { 
            Asset = it.Key, 
            Frames = ToFrames(it.Value) 
        }).ToList();

        return new EventMessage()
        {
            AngleEvents = [],
            SizeEvents = [],
            Created = frames,
            Disposed = [],
            PositionEvents = [],
        };
    }

    private IList<Frame> ToFrames(Dictionary<int, TransformFrame> pairs)
    {
        return pairs.Select(it => new Frame()
        {
            Position = new Vec2() { X = it.Value.Position.X, Y = it.Value.Position.Y },
            Angle = it.Value.Angle,
            Id = it.Key,
            Size = new Vec2() { X = it.Value.Size.X, Y = it.Value.Size.Y }
        }
        ).ToList();
    }
}
