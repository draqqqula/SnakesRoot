using ServerEngine.Models;
using SnakeGame.Mechanics.Frames;

namespace SnakeGame.Mechanics.ViewPort;

internal class ClientViewHandler
{
    private EventTable ClientPicture = new EventTable();
    private EventTable ClientInactive = new EventTable();
    public EventTable OnScreen { get; private set; } = new EventTable();
    public EventTable OutsideScreen { get; private set; } = new EventTable();
    public EventTable ToSleep { get; private set; } = new EventTable();

    private bool CheckLoaded(int id, EventEntry entry, HashSet<int> viewed)
    {
        return viewed.Contains(id) ||
        ((entry.Lifecycle == EventLifecycle.Dispose ||
        entry.Lifecycle == EventLifecycle.Renew) 
        && ClientInactive.Contains(id));
    }

    public void ApplyEvents(EventTable events, HashSet<int> visible)
    {
        var loadCondition = (int id, EventEntry entry) => CheckLoaded(id, entry, visible);

        var (globalInclude, globalExclude) = events.Split(loadCondition);

        var (localInclude, localExclude) = OutsideScreen.Split(loadCondition);

        ToSleep = GetSleeping(globalExclude, globalInclude);

        OutsideScreen = localExclude.Join(globalExclude);
        OnScreen = localInclude.Join(globalInclude);
        ClientPicture = ClientPicture.Join(OnScreen);
    }

    private EventTable GetSleeping(EventTable globalExclude, EventTable globalInclude)
    {
        var (fellAsleep, saveState) = ClientPicture.Split((id, _) =>
        {
            return globalExclude.Contains(id) &&
            !ClientInactive.Contains(id);
        });
        var (awaken, continuedSleep) = ClientInactive.Split((id, _) => globalInclude.Contains(id));
        ClientInactive = continuedSleep.Join(fellAsleep);

        return fellAsleep
            .Select(entry =>
            {
                entry.AddAction(EventLifecycle.Sleep);
                entry.Angle = null;
                entry.Position = null;
                entry.Asset = null;
                entry.Size = null;
            });
    }
}
