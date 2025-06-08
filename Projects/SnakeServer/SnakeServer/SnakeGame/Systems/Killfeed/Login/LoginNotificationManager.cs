using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Killfeed.Login;

internal class LoginNotificationManager
    (
    IRuntimeCommandFactory RuntimeCommandFactory,
    IClientRegistry ClientRegistry
    )
    : ISessionService
{
    public RuntimeCommand<Guid, bool> ShowLoginMessageCommand = new RuntimeCommand<Guid, bool>("ShowLoginMessage", RuntimeCommandFactory);

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        foreach (var client in ClientRegistry.Online)
        {
            ShowLoginMessageCommand.Send(client, id.Id, true);
        }
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
        foreach (var client in ClientRegistry.Online)
        {
            ShowLoginMessageCommand.Send(client, id.Id, false);
        }
    }
}
