namespace SnakeCore.MathExtensions.Hexagons;

public readonly struct HexagonTile
{
    public required int Q { get; init; }
    public required int R { get; init; }
    public int S => -Q - R;
}
