using SnakeGame.Systems.Modifiers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Modifiers;

internal abstract class Modifier<T> : IModifiable<T>
{
    private IModifiable<T> _inner;
    public Modifier(IModifiable<T> inner)
    {
        _inner = inner;
    }
    public T Value => Enabled ? Modify(_inner.Value) : _inner.Value;
    public bool Enabled { get; set; }
    protected abstract T Modify(T initial);
}
