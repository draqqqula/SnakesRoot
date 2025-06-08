using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;

namespace ServerEngine.Routine;

public interface IRoutineService : IUpdateService
{
    public abstract IRoutine Routine { get; }
}

public abstract class RoutineService<T> : IRoutineService where T : IRoutine, new()
{
    public IRoutine Routine => new T();

    public abstract void Update(IGameContext context);
}
