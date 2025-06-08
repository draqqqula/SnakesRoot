using System.Numerics;

namespace SnakeCore.MathExtensions.Hexagons;

public class HexagonGrid
{
    public float InscribedRadius { get; private init; }
    public float CellSide { get; private init; }
    public float SegmentHeight { get; private init; }

    public HexagonGrid(float inscribedRadius)
    {
        InscribedRadius = inscribedRadius;
        CellSide = InscribedRadius * 2 / MathF.Sqrt(3);
        SegmentHeight = (CellSide * 2 - CellSide) / 2;
    }

    public Vector2 Translate(HexagonTile point)
    {
        return new Vector2(point.Q * InscribedRadius + point.R * InscribedRadius * 2, point.Q * CellSide * 1.5f);
    }
}
