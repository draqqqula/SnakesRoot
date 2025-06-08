using MessageSchemes;
using Microsoft.Win32;
using SnakeGame.Mechanics.Bodies;
using System.Numerics;

namespace SnakeGame.Mechanics.Frames.Output.Interfaces;

internal interface INotificationListener
{
    public void NotifyPositionChanged(int id, Vector2 position);

    public void NotifySizeChanged(int id, Vector2 size);

    public void NotifyAngleChanged(int id, float angle);

    public void NotifyCreated(int id, string asset, Transform transform);

    public void NotifyDisposed(int id);

    public void NotifyTransformed(int id, string newAsset);
}
