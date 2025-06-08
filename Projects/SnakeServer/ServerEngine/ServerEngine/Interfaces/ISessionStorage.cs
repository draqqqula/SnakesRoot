namespace ServerEngine.Interfaces;

public interface ISessionStorage<T>
{
    public IEnumerable<T> GetAll();

    public ISessionManager? GetById(T id);

    public T Add(ISessionManager manager);

    public void Remove(Guid id);
}
