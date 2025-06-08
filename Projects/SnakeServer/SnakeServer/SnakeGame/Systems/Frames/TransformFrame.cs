using System.Numerics;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Frames.Output.Interfaces;

namespace SnakeGame.Mechanics.Frames;

internal class TransformFrame : TransformBase
{
    private readonly INotificationListener _listener;
    private readonly int _id;
    private Vector2 _position = Vector2.Zero;
    private Vector2 _size = Vector2.One;
    private float _angle = 0;

    public TransformFrame(INotificationListener repository, int id, Transform transform)
    {
        _listener = repository;
        _id = id;
        _position = transform.Position;
        _size = transform.Size;
        _angle = transform.Angle;
    }

    public override int? Id { get => _id; }

    public override Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _listener.NotifyPositionChanged(_id, value);
                _position = value;
            }
        }
    }
    public override float Angle
    {
        get => _angle;
        set
        {
            if (_angle != value)
            {
                _listener.NotifyAngleChanged(_id, value);
                _angle = value;
            }
        }
    }
    public override Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _listener.NotifySizeChanged(_id, value);
                _size = value;
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _listener.NotifyDisposed(_id);
    }

    public override void ChangeAsset(string asset)
    {
        _listener.NotifyTransformed(_id, asset);
    }
}
