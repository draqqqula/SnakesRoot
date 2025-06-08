using SnakeGame.Common;
using SnakeGame.Mechanics.Bodies;
using SnakeGame.Mechanics.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.GameObjects.Characters;

internal class BodyPartFactory(FrameFactory FrameFactory) : IBodyPartFactory
{
    public ScoreSegment Create(Transform transform, byte tier, TeamColor team)
    {
        return new ScoreSegment()
        {
            Transform = FrameFactory.Create($"body{tier}_{team}", transform),
            Team = team,
            Tier = tier
        };
    }
}
