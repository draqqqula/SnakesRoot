using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.PowerUps;

internal enum ItemState
{
    Inactive,
    Pickup,
    Carried,
    Claimed
}
