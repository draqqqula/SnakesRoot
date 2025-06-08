using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;

namespace ServerEngine.Routine;

internal class RoutineHandler
{
    private readonly IRoutine[] _listedRoutines;
    private readonly IServiceProvider _provider;
    private readonly Dictionary<IRoutine, IEnumerable<IRoutineService>> _map;

    private List<IRoutine> Unchecked = [];

    public RoutineHandler(IServiceProvider provider)
    {
        _provider = provider;
        _listedRoutines = provider.GetServices<IRoutine>().ToArray();

        _map = provider.GetServices<IRoutineService>()
            .GroupBy(it => it.Routine)
            .ToDictionary(it => it.Key, it => it.AsEnumerable());
    }

    public void Restart()
    {
        Unchecked = _listedRoutines.ToList();
    }

    public void Invoke(IGameContext context)
    {
        if (Unchecked.Count == 0) 
        {
            return;
        }
        while (TryTakeOne(out var routine))
        {
            if (routine is not null && 
                _map.TryGetValue(routine, out var services))
            {
                foreach (var service in services)
                {
                    service.Update(context);
                }
            }
        }
    }

    private bool TryTakeOne(out IRoutine? routine)
    {
        for (int i = 0; i < Unchecked.Count; i++)
        {
            if (Unchecked[i].IsReady(_provider))
            {
                Unchecked.RemoveAt(i);
                routine = Unchecked[i];
                return true;
            }
        }
        routine = default;
        return false;
    }
}
