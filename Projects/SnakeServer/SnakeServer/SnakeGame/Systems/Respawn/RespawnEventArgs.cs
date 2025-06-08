using ServerEngine.Models;
using SnakeGame.Systems.GameObjects.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Respawn;

internal readonly struct RespawnEventArgs
{
    public required ClientIdentifier Client { get; init; }
    public required SnakeCharacter Character { get; init; }
}
