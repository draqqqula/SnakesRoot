using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Collision;
using SnakeGame.Mechanics.Collision.Resolvers;
using SnakeGame.Mechanics.Collision.Shapes;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Systems.ViewPort.Interfaces;
using System.Numerics;

namespace SnakeGame.Mechanics.ViewPort;

internal class ViewPortManager(
    Dictionary<ClientIdentifier, ClientViewPort> ViewPorts, 
    FrameStorage Storage,
    ICollisionResolver<AABB, AABB> Collision,
    FrameFactory Factory
    ) : 
    ISessionService, ITrackingSource
{
    public const float ViewPortSize = 200f;
    public IEnumerable<int> GetTracked(ClientIdentifier id)
    {
        var view = ViewPorts[id];
        foreach (var frame in Storage.GetAll())
        {
            if (Collision.IsColliding(view.GetLayout(), new AABB()
            {
                Min = frame.Transform.Position - frame.Transform.Size * 0.5f,
                Max = frame.Transform.Position + frame.Transform.Size * 0.5f
            }))
            {
                yield return frame.Id;
            }
        }
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        ViewPorts.Add(id, new ClientViewPort() 
        { 
            Transform = new TransformObject() 
            { 
                Angle = 0f,
                Position = Vector2.Zero,
                Size = Vector2.One * ViewPortSize,
            }
        });
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        ViewPorts.Remove(id);
    }
}
