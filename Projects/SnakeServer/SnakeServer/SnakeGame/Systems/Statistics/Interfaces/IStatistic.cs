using ServerEngine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Statistics.Interfaces;

internal interface IStatistic<T>
{
    public void Change(ClientIdentifier id, Func<T, T> func);
    public bool TryGetValue(ClientIdentifier id, [MaybeNullWhen(false)] out T value);
}
