using ServerEngine.Models;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.RuntimeCommands;

internal class RuntimeCommand<T>
{
    private readonly IRuntimeCommandSender<T, ICompleted> _sender;
    public RuntimeCommand(string name, IRuntimeCommandFactory factory)
    {
        _sender = factory.Declare<T>(name).Build();
    }

    public void Send(ClientIdentifier clientId, T arg)
    {
        _sender.Call().Pass(arg).To(clientId);
    }
}

internal class RuntimeCommand<T1, T2>
{
    private readonly IRuntimeCommandSender<T1, ILink<T2, ICompleted>> _sender;
    public RuntimeCommand(string name, IRuntimeCommandFactory factory)
    {
        _sender = factory.Declare<T2>(name).AddArgument<T1>().Build();
    }

    public void Send(ClientIdentifier clientId, T1 arg1, T2 arg2)
    {
        _sender.Call().Pass(arg1).Pass(arg2).To(clientId);
    }
}

internal class RuntimeCommand<T1, T2, T3>
{
    private readonly IRuntimeCommandSender<T1, ILink<T2, ILink<T3, ICompleted>>> _sender;
    public RuntimeCommand(string name, IRuntimeCommandFactory factory)
    {
        _sender = factory.Declare<T3>(name).AddArgument<T2>().AddArgument<T1>().Build();
    }

    public void Send(ClientIdentifier clientId, T1 arg1, T2 arg2, T3 arg3)
    {
        _sender.Call().Pass(arg1).Pass(arg2).Pass(arg3).To(clientId);
    }
}

internal class RuntimeCommand<T1, T2, T3, T4>
{
    private readonly IRuntimeCommandSender<T1, ILink<T2, ILink<T3, ILink<T4, ICompleted>>>> _sender;
    public RuntimeCommand(string name, IRuntimeCommandFactory factory)
    {
        _sender = factory.Declare<T4>(name).AddArgument<T3>().AddArgument<T2>().AddArgument<T1>().Build();
    }

    public void Send(ClientIdentifier clientId, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        _sender.Call().Pass(arg1).Pass(arg2).Pass(arg3).Pass(arg4).To(clientId);
    }
}

internal class RuntimeCommand<T1, T2, T3, T4, T5>
{
    private readonly IRuntimeCommandSender<T1, ILink<T2, ILink<T3, ILink<T4, ILink<T5, ICompleted>>>>> _sender;
    public RuntimeCommand(string name, IRuntimeCommandFactory factory)
    {
        _sender = factory.Declare<T5>(name).AddArgument<T4>().AddArgument<T3>().AddArgument<T2>().AddArgument<T1>().Build();
    }

    public void Send(ClientIdentifier clientId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        _sender.Call().Pass(arg1).Pass(arg2).Pass(arg3).Pass(arg4).Pass(arg5).To(clientId);
    }
}

internal class RuntimeCommand<T1, T2, T3, T4, T5, T6>
{
    private readonly IRuntimeCommandSender<T1, ILink<T2, ILink<T3, ILink<T4, ILink<T5, ILink<T6, ICompleted>>>>>> _sender;
    public RuntimeCommand(string name, IRuntimeCommandFactory factory)
    {
        _sender = factory.Declare<T6>(name).AddArgument<T5>().AddArgument<T4>().AddArgument<T3>().AddArgument<T2>().AddArgument<T1>().Build();
    }

    public void Send(ClientIdentifier clientId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        _sender.Call().Pass(arg1).Pass(arg2).Pass(arg3).Pass(arg4).Pass(arg5).Pass(arg6).To(clientId);
    }
}