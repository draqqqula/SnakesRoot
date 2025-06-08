using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Services.Output;

namespace SnakeGame.Services;

internal class CommandSender : IOutputService<ClientCommandWrapper>
{
    private List<(int, ClientCommandWrapper)> Payload = [];
    public IEnumerable<ClientCommandWrapper> Pass()
    {
        var image = Payload.OrderBy(it => it.Item1).ToArray();
        Payload.Clear();
        foreach (var item in image)
        {
            yield return item.Item2;
        }
    }

    public void Send(ISerializableCommand command, ClientIdentifier clientId, int priority)
    {
        Payload.Add((priority, new ClientCommandWrapper() 
        { 
            Command = command, 
            Id = clientId 
        }));
    }
}
