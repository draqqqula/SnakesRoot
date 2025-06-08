using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.GameObjects.Characters;
using SnakeGame.Systems.Killfeed;
using SnakeGame.Systems.Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Player.Origin;

internal class OriginAssigner(IServiceProvider provider) : 
    IEventListener<RespawnEventArgs>, IEventListener<KillInteraction>, ISessionService
{
    private IDictionary<Guid, int> _origins = provider.GetRequiredKeyedService<IDictionary<Guid, int>>("origins");
    public void OnFired(KillInteraction arguments)
    {
        _origins.Remove(arguments.Victim.Id);
    }

    public void OnFired(RespawnEventArgs arguments)
    {
        var id = arguments.Character.Head.Transform.Id;
        if (id is null)
        {
            return;
        }
        _origins[arguments.Client.Id] = id.Value;
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        _origins.Remove(id.Id);
    }
}
