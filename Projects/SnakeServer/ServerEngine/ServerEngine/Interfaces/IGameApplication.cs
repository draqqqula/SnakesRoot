using ServerEngine.Models;

namespace ServerEngine.Interfaces;

public interface IGameApplication
{
    public Task<ISessionManager> CreateSessionAsync(ISessionLauncher launcher);
}
