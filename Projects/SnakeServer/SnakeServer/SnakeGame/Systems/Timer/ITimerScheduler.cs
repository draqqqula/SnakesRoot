using ServerEngine.Interfaces;

namespace SnakeGame.Systems.Timer;

internal interface ITimerScheduler : IGameManager
{
    public void Set(TimeSpan duration, Action expression);
}
