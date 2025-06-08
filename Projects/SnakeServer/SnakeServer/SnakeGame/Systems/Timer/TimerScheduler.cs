
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;

namespace SnakeGame.Systems.Timer;

internal class TimerScheduler : ITimerScheduler, IUpdateService
{
    private TimeSpan t;
    private List<(TimeSpan, Action)> Scheduled = [];
    public void Set(TimeSpan duration, Action expression)
    {
        Scheduled.Add((t + duration, expression));
    }

    public void Update(IGameContext context)
    {
        t += TimeSpan.FromSeconds(context.DeltaTimeF);
        for (int i = Scheduled.Count - 1; i >= 0; i--)
        {
            if (Scheduled[i].Item1 <= t)
            {
                Scheduled[i].Item2();
                Scheduled.RemoveAt(i);
            }
        }
    }
}
