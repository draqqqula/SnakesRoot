using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Serialization;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeCore.Collections;
using SnakeGame.Services;
using SnakeGame.Services.Output;
using SnakeGame.Services.Output.Commands;
using SnakeGame.Systems.Service;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.RuntimeCommands;

internal class RuntimeCommandAggregator
    (
    
    IClientRegistry Registry
    
    ) : 
    IRuntimeCommandAggregator, ISessionService, IOutputService<ClientCommandWrapper>
{
    private readonly ItemRegistry<string, byte> _availableIds = new ItemRegistry<string, byte>(Enumerable.Range(0, 256).Select(it => (byte)it).GetEnumerator());
    private readonly Dictionary<string, byte> _declared = [];
    private readonly List<ClientCommandWrapper> _commandBuffer = [];

    public void Declare(string name)
    {
        var id = _availableIds.GetKey(name);
        _declared.Add(name, id);

        foreach (var client in Registry.Online)
        {
            SendDeclare(client, name, id);
        }
    }

    public void Send(string name, ISerializableCommand command, ClientIdentifier client)
    {
        if (_declared.TryGetValue(name, out var commandId))
        {
            SendExecute(client, commandId, command);
        }
    }

    private void SendExecute(ClientIdentifier client, byte variableId, ISerializableCommand command)
    {
        _commandBuffer.Add(new ClientCommandWrapper()
        {
            Command = new ExecuteRuntimeCommandCommand()
            {
                IID = variableId,
                RuntimeCommand = command
            },
            Id = client
        });
    }

    private void SendDeclare(ClientIdentifier id, string name, byte commandId)
    {
        _commandBuffer.Add(new ClientCommandWrapper()
        {
            Command = new DeclareRuntimeCommandCommand()
            {
                Name = name,
                IID = commandId
            },
            Id = id
        });
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        foreach (var tracker in _declared)
        {
            SendDeclare(id, tracker.Key, tracker.Value);
        }
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
    }

    public IEnumerable<ClientCommandWrapper> Pass()
    {
        var data = _commandBuffer.ToArray();
        _commandBuffer.Clear();
        return data;
    }
}
