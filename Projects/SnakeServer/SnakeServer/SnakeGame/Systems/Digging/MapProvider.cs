using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeCore.MathExtensions.Hexagons;
using SnakeGame.Systems.Digging.Interfaces;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

internal class MapProvider
    (
    
    IMapLoader Loader, 
    IGameConfiguration Configuration, 
    IRuntimeCommandFactory RuntimeCommandFactory
    
    ) : IMapProvider, ISessionService, IStartUpService
{
    private readonly string[] MapPool = Configuration.Get<string[]>("MapPool");
    private readonly float TileSize = 1;
    private readonly IMapLoader Loader = Loader;
    private readonly Random Random = new Random();
    private readonly RuntimeCommand<string, float> LoadMapCommand = new RuntimeCommand<string, float>("LoadMap", RuntimeCommandFactory);
    private string MapName;
    public HexagonBitMap Map { get; private set; }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        LoadMapCommand.Send(id, MapName, TileSize);
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
    }

    public void Start(IGameContext context)
    {
        var randomIndex = Random.Next(0, MapPool.Length);
        MapName = MapPool[randomIndex];
        Map = Loader.GetMap(MapName);
    }
}
