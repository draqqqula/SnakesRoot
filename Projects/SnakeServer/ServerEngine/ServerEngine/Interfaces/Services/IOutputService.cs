using ServerEngine.Interfaces.Output;
using ServerEngine.Models;

namespace ServerEngine.Interfaces.Services;

public interface IOutputService<T>
{
    public IEnumerable<T> Pass();
}
