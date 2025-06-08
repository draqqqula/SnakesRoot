using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Modifiers.Interfaces;

internal interface IModifiable<T>
{
    public T Value { get; }
}
