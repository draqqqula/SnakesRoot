using ServerEngine.Interfaces;

namespace ServerEngine;

public class SessionStorage : ISessionStorage<Guid>
{
    private readonly Dictionary<Guid, ISessionManager> Storage = [];
    public Guid Add(ISessionManager manager)
    {
        var id = Guid.NewGuid();
        Storage.Add(id, manager);
        return id;
    }

    public IEnumerable<Guid> GetAll()
    {
        return Storage.Keys;
    }

    public ISessionManager? GetById(Guid id)
    {
        return Storage.GetValueOrDefault(id);
    }

    public void Remove(Guid id)
    {
        Storage.Remove(id);
    }
}
