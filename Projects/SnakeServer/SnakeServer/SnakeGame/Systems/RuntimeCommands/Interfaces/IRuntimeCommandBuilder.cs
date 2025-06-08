using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Serialization;
using ServerEngine.Models;
using SnakeGame.Services;
using SnakeGame.Services.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.RuntimeCommands.Interfaces;

internal interface IRuntimeCommandAggregator
{
    public void Declare(string name);
    public void Send(string name, ISerializableCommand command, ClientIdentifier clientId);
}

internal interface IRuntimeCommandFactory
{
    public IRuntimeCommandBuilder<T, ICompleted> Declare<T>(string name);
}

internal interface IRuntimeCommandBuilder<TArg, TPrevious>
{
    public IRuntimeCommandBuilder<T, ILink<TArg, TPrevious>> AddArgument<T>();
    public RuntimeCommandSender<TArg, TPrevious> Build();
}

internal interface IRuntimeCommandSender<TArg, TPrevious>
{
    public ILink<TArg, TPrevious> Call();
}

internal interface ILink<TArg, TNext>
{
    public TNext Pass(TArg argument);
}

internal interface ICompleted
{
    public ICompleted To(ClientIdentifier id);
    public ICompleted BroadCast(ClientIdentifier[] clients)
    {
        foreach (var client in clients)
        {
            To(client);
        }
        return this;
    }
}

internal class RuntimeCommandStartUpFactory(IServiceProvider Provider, IRuntimeCommandAggregator Director) : IRuntimeCommandFactory
{
    public IRuntimeCommandBuilder<T, ICompleted> Declare<T>(string name)
    {
        Director.Declare(name);
        var container = new CommandContainer();
        var completed = new RuntimeCommandCompleted(name, container, Director);
        var serializer = Provider.GetRequiredService<IBinarySerializer<T>>();
        var link = new RuntimeCommandDeclarerLink<T, ICompleted>(completed, container, serializer, Provider);
        return link;
    }
}

internal class RuntimeCommandDeclarerLink<TArg, TPrevious>
    (

    TPrevious Previous,
    CommandContainer Container,
    IBinarySerializer<TArg> Serializer,
    IServiceProvider Provider

    ) : IRuntimeCommandBuilder<TArg, TPrevious>
{
    private ILink<TArg, TPrevious> Link => new RuntimeCommandArgumentLink<TArg, TPrevious>(Previous, Serializer, Container);
    public IRuntimeCommandBuilder<T, ILink<TArg, TPrevious>> AddArgument<T>()
    {
        var newSerializer = Provider.GetRequiredService<IBinarySerializer<T>>();
        var declarer = new RuntimeCommandDeclarerLink<T, ILink<TArg, TPrevious>>(Link, Container, newSerializer, Provider);
        return declarer;
    }

    public RuntimeCommandSender<TArg, TPrevious> Build()
    {
        return new RuntimeCommandSender<TArg, TPrevious>(Link, Container);
    }
}

internal class RuntimeCommandSender<TArg, TPrevious>(ILink<TArg, TPrevious> Link, CommandContainer Container) : IRuntimeCommandSender<TArg, TPrevious>
{
    public ILink<TArg, TPrevious> Call()
    {
        Container.Command = null;
        return Link;
    }
}

internal class RuntimeCommandArgumentLink<TArg, TNext>(TNext Next, IBinarySerializer<TArg> Serializer, CommandContainer Container) : ILink<TArg, TNext>
{
    public TNext NextLink = Next;
    public TNext Pass(TArg argument)
    {
        Container.Command = new PrefixedCommand<TArg>(Serializer)
        {
            Command = Container.Command,
            Value = argument
        };
        return NextLink;
    }
}

internal class CommandContainer
{
    public ISerializableCommand? Command;
}

internal class RuntimeCommandCompleted
    (

    string Name,
    CommandContainer Container,
    IRuntimeCommandAggregator Director

    ) 
    : ICompleted
{
    public ICompleted To(ClientIdentifier id)
    {
        if (Container.Command is null)
        {
            return this;
        }
        Director.Send(Name, Container.Command, id);
        return this;
    }
}