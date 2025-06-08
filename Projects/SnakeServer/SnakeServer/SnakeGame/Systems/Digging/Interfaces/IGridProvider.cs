using SnakeCore.MathExtensions.Hexagons;

namespace SnakeGame.Systems.Digging.Interfaces;

internal interface IGridProvider
{
    public HexagonGrid Grid { get; }
}
