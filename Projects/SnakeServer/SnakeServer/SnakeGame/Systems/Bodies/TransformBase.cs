using System.Numerics;

namespace SnakeGame.Mechanics.Bodies
{
    internal abstract class TransformBase : IDisposable
    {
        public event Action? OnDisposed;
        public bool IsDisposed { get; private set; } = false;
        public virtual int? Id { get; } = null;
        public virtual Vector2 Position { get; set; }
        public virtual float Angle { get; set; }
        public virtual Vector2 Size { get; set; }

        public Transform ReadOnly => new Transform()
        {
            Angle = Angle,
            Position = Position,
            Size = Size,
        };

        public virtual void Dispose()
        {
            IsDisposed = true;
            OnDisposed?.Invoke();
        }

        public abstract void ChangeAsset(string newAsset);
    }
}
