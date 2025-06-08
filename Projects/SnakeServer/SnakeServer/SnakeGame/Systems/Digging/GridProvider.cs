using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using SnakeCore.MathExtensions.Hexagons;
using SnakeGame.Systems.Digging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

internal class GridProvider(IGameConfiguration Configuration) : IGridProvider, IStartUpService
{
    public HexagonGrid Grid { get; private set; }
    public void Start(IGameContext context)
    {
        Grid = new HexagonGrid(Configuration.Get<float>("MapTileSize"));
    }
}
