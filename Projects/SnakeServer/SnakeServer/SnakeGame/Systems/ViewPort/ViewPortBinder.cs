using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeCore.MathExtensions;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Models.Gameplay;
using SnakeGame.Systems.ViewPort.Interfaces;

namespace SnakeGame.Mechanics.ViewPort;

internal class ViewPortBinder(
    Dictionary<ClientIdentifier, ClientViewPort> ViewPorts) : IUpdateService, IViewPortBinder
{
    class Binding : IDisposable
    {
        public readonly ClientIdentifier ClientId;
        public readonly TransformBase Target;
        public Binding(ClientIdentifier clientId, TransformBase target)
        {
            ClientId = clientId;
            Target = target;
            Target.OnDisposed += Dispose;
        }

        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            Target.OnDisposed -= Dispose;
            IsDisposed = true;
        }
    }

    private const float InterpolationFactor = 0.3f;
    private Dictionary<ClientIdentifier, Binding> Bindings = [];
    public void Bind(ClientIdentifier id, TransformBase target)
    {
        Reset(id);
        var binding = new Binding(id, target);
        Bindings[id] = binding;
    }

    public void Reset(ClientIdentifier id)
    {
        if (Bindings.TryGetValue(id, out var binding))
        {
            binding.Dispose();
        }
    }

    public void Update(IGameContext context)
    {
        ClearDisposed();
        UpdatePosition(context.DeltaTimeF);
    }

    private void UpdatePosition(float deltaTime)
    {
        foreach (var view in ViewPorts)
        {
            if (Bindings.TryGetValue(view.Key, out var binding))
            {
                var x = MathEx.Lerp(view.Value.Transform.Position.X, binding.Target.Position.X, InterpolationFactor, deltaTime);
                var y = MathEx.Lerp(view.Value.Transform.Position.Y, binding.Target.Position.Y, InterpolationFactor, deltaTime);
                view.Value.Transform.Position = new System.Numerics.Vector2(x, y);
                view.Value.Enabled = true;
            }
        }
    }

    private void ClearDisposed()
    {
        foreach (var binding in Bindings.ToArray())
        {
            if (binding.Value.IsDisposed)
            {
                Bindings.Remove(binding.Key);
            }
        }
    }
}
