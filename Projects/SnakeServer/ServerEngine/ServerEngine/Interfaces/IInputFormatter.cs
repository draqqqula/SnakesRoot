using ServerEngine.Models;

namespace ServerEngine.Interfaces;

public interface IInputFormatter<T>
{
    public ResolveResult TryResolve(T input, ClientIdentifier id);
}
