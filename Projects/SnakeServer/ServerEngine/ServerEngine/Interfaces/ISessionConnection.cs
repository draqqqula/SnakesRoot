using ServerEngine.Models.Input;
using ServerEngine.Models;
using ServerEngine.Interfaces.Output;

namespace ServerEngine.Interfaces;

public interface ISessionConnection : IDisposable
{
    public bool Closed { get; }
    public Task<T?> GetOutputAsync<T>();
    public void SendInput<T>(T data);
}
