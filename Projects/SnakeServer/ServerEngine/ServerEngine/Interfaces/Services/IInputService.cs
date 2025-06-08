using ServerEngine.Models;

namespace ServerEngine.Interfaces.Services;

public interface IInputService<T>
{
    public void OnInput(ClientIdentifier id, T data);
}
