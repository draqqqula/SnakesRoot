using ServerEngine.Models;

namespace ServerEngine.Interfaces;

public interface IGameState
{
    public void Initialize(ISessionLauncher launcher);
    public void Update(TimeSpan deltaTime);
}