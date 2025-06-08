using SnakeGame.Systems.Modifiers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Modifiers;

internal class ModifiableValue<T> : IModifiable<T>
{
    public ModifiableValue(T value)
    {
        Value = value;
    }
    public T Value { get; }
}
