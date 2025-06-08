using ServerEngine.Models;
using SnakeGame.Mechanics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.ViewPort.Interfaces
{
    internal interface IViewPortBinder
    {
        public void Bind(ClientIdentifier id, TransformBase target);
        public void Reset(ClientIdentifier id);
    }
}
