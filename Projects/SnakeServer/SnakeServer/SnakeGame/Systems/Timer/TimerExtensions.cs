namespace SnakeGame.Systems.Timer;

internal static class TimerExtensions
{
    public static void SetSeconds(this ITimerScheduler scheduler, float seconds, Action expression)
    {
        scheduler.Set(TimeSpan.FromSeconds(seconds), expression);
    }
}
