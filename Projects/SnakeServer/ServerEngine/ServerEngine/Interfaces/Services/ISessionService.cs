using ServerEngine.Models;

namespace ServerEngine.Interfaces.Services;

public interface ISessionService
{
    public void OnJoin(IGameContext context, ClientIdentifier id);
    public void OnLeave(IGameContext context, ClientIdentifier id);
}
