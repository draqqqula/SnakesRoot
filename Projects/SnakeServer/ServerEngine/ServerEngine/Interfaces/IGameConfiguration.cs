namespace ServerEngine.Interfaces;

public interface IGameConfiguration : IGameManager
{
    public T? Get<T>(string name);
}
