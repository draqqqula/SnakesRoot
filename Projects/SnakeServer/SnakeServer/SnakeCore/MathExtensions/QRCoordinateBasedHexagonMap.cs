using SnakeCore.MathExtensions.Hexagons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeCore.MathExtensions;

public class QRCoordinateBasedHexagonMap : HexagonBitMap
{
    private readonly int _minQ;
    private readonly int _maxQ;
    private readonly int _minR;
    private readonly int _maxR;
    public QRCoordinateBasedHexagonMap(BitArray data, int minQ, int minR, int maxQ, int maxR) : base(data)
    {
        _minQ = minQ;
        _maxQ = maxQ;
        _minR = minR;
        _maxR = maxR;
    }

    public override bool TryGetIndex(int q, int r, out int index)
    {
        var half = Convert.ToInt32(MathF.Floor((float)q / 2));
        var r0 = r + half;
        if (q < _minQ || q >= _maxQ || r0 < _minR || r0 >= _maxR)
        {
            index = 0;
            return false;
        }
        index = (q - _minQ) * (_maxR - _minR) + r0 - _minR;
        return true;
    }
}
