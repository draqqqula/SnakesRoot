using SnakeGame.Systems.EventPipeline;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Killfeed;

internal class KillFeedManager
    (
    IRuntimeCommandFactory RuntimeCommandFactory,
    IClientRegistry ClientRegistry
    ) : IEventListener<KillInteraction>
{
    private readonly RuntimeCommand<Guid, Guid> _showKillMessageCommand = new RuntimeCommand<Guid, Guid>("ShowKillMessage", RuntimeCommandFactory);
    public void OnFired(KillInteraction arguments)
    {
        foreach (var client in ClientRegistry.Online)
        {
            _showKillMessageCommand.Send(client, arguments.Killer.Id, arguments.Victim.Id);
        }
    }
}
