using ServerEngine.Interfaces;
using ServerEngine.Models;
using ServerEngine.Models.Input;
using ServerEngine.Profiling.Interfaces;
using System.Diagnostics;
using System.Threading;

namespace ServerEngine;

internal class SessionManager : ISessionManager
{
    private const int FixedTimeStep = 16;
    private readonly Task _loopTask;
    private readonly SessionHandler _handler;
    private readonly IProfileAggregator _profileAggregator;

    public int ConnectedCount => _handler.PlayerCounter;

    public SessionManager(SessionHandler handler, GameState state, IProfileAggregator profileAggregator)
    {
        _handler = handler;
        _loopTask = Task.Run(() => LoopUpdateAsync(state, handler).ContinueWith((task) => { OnClosed.Invoke(); }));
        _profileAggregator = profileAggregator;
    }

    public event Action OnClosed = delegate { };

    public void Close()
    {
        _handler.Closed = true;
    }

    public async Task<ISessionConnection> ConnectAsync(ClientIdentifier id)
    {
        _handler.JoinQueue.Enqueue(id);
        _handler.PlayerCounter += 1;
        return new SessionConnection(_handler, id);
    }

    public SessionStatus GetStatus()
    {
        switch (_loopTask.Status)
        {
            case TaskStatus.Running: return SessionStatus.Running;
            case TaskStatus.Created: return SessionStatus.Pending;
            case TaskStatus.RanToCompletion: return SessionStatus.Completed;
            case TaskStatus.WaitingForActivation: return SessionStatus.Running;
            case TaskStatus.WaitingToRun: return SessionStatus.Pending;
            case TaskStatus.Faulted: return SessionStatus.Aborted;
            case TaskStatus.WaitingForChildrenToComplete: return SessionStatus.Completed;
            case TaskStatus.Canceled: return SessionStatus.Aborted;
            default: return SessionStatus.Unknown;
        }
    }
    private static async Task LoopUpdateAsync(GameState state, SessionHandler handler)
    {
        long extraTime = 0;
        DateTime start = DateTime.UtcNow;
        TimeSpan deltaTime = TimeSpan.FromMilliseconds(FixedTimeStep);
        long timerError = 0;
        long totalFrames = 0;
        try
        {
            var stopWatch = Stopwatch.StartNew();
            while (!handler.Closed)
            {
                timerError = (stopWatch.Elapsed - deltaTime).Ticks;
                stopWatch.Restart();

                await handler.Semaphore.WaitAsync();

                state.Update(deltaTime * handler.TimeScale);
                totalFrames += 1;

                extraTime += deltaTime.Ticks - stopWatch.ElapsedTicks;

                if (extraTime > 0)
                {
                    Thread.Sleep(TimeSpan.FromTicks(extraTime));
                    extraTime = 0;
                }
                handler.Semaphore.Release();
            }
            Console.WriteLine($"Session lasted for {(DateTime.UtcNow - start).TotalSeconds} seconds. Total frames: {totalFrames}");
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Session closed due to unhandled exception \"{ex.Message}\"{ex.StackTrace}");
        }
    }
}