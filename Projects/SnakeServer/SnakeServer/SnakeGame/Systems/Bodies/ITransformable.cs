using SnakeGame.Mechanics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Bodies;

internal interface ITransformable
{
    public TransformBase Transform { get; }
}
