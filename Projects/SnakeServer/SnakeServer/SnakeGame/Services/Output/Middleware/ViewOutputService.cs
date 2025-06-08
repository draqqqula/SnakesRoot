using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Mechanics.Frames;
using SnakeGame.Mechanics.Frames.Output.Interfaces;
using SnakeGame.Mechanics.ViewPort;
using SnakeGame.Models.Output.Internal;
using SnakeGame.Services.Output.Commands;
using SnakeGame.Systems.Service;
using SnakeGame.Systems.ViewPort;

namespace SnakeGame.Services.Output.Services;

internal class ViewOutputService(IClientRegistry Registry, TrackingAggregator Aggregator, FrameStorage Storage, ITableProvider Provider) :
    IOutputService<ClientCommandWrapper>
{
    private readonly Dictionary<ClientIdentifier, ClientViewHandler> _views = [];

    public IEnumerable<ClientCommandWrapper> Pass()
    {
        var globalChanges = Provider.Take();
        var state = Storage.GetAll();

        foreach (var client in Registry.Online)
        {
            var tracked = Aggregator.GetTracked(client).ToHashSet();

            ClientViewHandler? handler;

            if (_views.TryGetValue(client, out handler))
            {
                handler.ApplyEvents(globalChanges, tracked);
            }
            else
            {
                handler = new ClientViewHandler();
                handler.ApplyEvents(EventTable.FromState(state), tracked);
                _views.Add(client, handler);
            }

            yield return new ClientCommandWrapper()
            {
                Id = client,
                Command = new EventSourcingCommand()
                {
                    Table = handler.OnScreen.Join(handler.ToSleep)
                }
            };
        }
    }
}
