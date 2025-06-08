using System.Numerics;
using MessageSchemes;
using SnakeCore.Collections;

namespace SnakeGame.Models.Output.Internal;

internal class FrameWrapper : MessageSchemes.Frame
{
    public required Vector2 FramePosition
    {
        init => this.Position = new Vec2() { X = value.X, Y = value.Y };
    }
    public required float FrameAngle
    {
        init => this.Angle = value;
    }
    public required Vector2 FrameSize
    {
        init => this.Size = new Vec2() { X = value.X, Y = value.Y };
    }
    public required int FrameId
    {
        init => this.Id = value;
    }
}
