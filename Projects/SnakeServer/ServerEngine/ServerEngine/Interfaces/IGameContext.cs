namespace ServerEngine.Interfaces;

public interface IGameContext
{
    public float DeltaTimeF { get; }
    public TimeSpan DeltaTime { get; }
    public T Using<T>();
}
