using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.Characters;

internal interface IBodyPartFactory
{
    public ScoreSegment Create(Transform transform, byte tier, TeamColor team);
}
