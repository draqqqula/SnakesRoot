using SnakeCore.MathExtensions.Hexagons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IMapLoader
{
    HexagonBitMap GetMap(string name);
}