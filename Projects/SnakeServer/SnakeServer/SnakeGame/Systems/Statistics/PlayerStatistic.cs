using ServerEngine.Models;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Statistics;

internal class PlayerStatistic<T>(RuntimeCommand<T> Command, T DefaultValue) : IStatistic<T>
{
    private readonly Dictionary<ClientIdentifier, T> _values = [];
    public void Change(ClientIdentifier id, Func<T, T> func)
    {
        T value;
        if (_values.TryGetValue(id, out value))
        {
            _values[id] = func(value);
        }
        else
        {
            value = DefaultValue;
            _values[id] = func(DefaultValue);
        }
        Command.Send(id, value);
    }

    public bool TryGetValue(ClientIdentifier id, [MaybeNullWhen(false)] out T value)
    {
        return _values.TryGetValue(id, out value);
    }
}
