using SnakeCore.MathExtensions.Hexagons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging.Interfaces;

internal interface IMapProvider
{
    public HexagonBitMap Map { get; }
}
