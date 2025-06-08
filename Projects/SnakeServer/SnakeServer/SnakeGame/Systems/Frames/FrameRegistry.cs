using SnakeCore.Collections;

namespace SnakeGame.Mechanics.Frames;

internal class FrameRegistry : KeyRegistry<int>
{
    public FrameRegistry() : base(

        Enumerable.Range(0, int.MaxValue)
        .Concat(Enumerable.Range(int.MinValue, int.MaxValue))
        .GetEnumerator())

    {
    }
}
