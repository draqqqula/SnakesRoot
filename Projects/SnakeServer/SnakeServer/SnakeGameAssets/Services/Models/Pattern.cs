using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameAssets.Services.Models;

public class Pattern
{
    private readonly Vector2[] _points;
    public Pattern(Vector2[] points)
    {
        _points = points;
    }

    public IEnumerable<Vector2> Transformed(float angle, float scale, Vector2 offset)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);
        foreach (var point in _points)
        {
            var x1 = point.X * cos - point.Y * sin;
            var y1 = point.Y * cos + point.X * sin;
            yield return new Vector2(x1 * scale + offset.X, y1 * scale + offset.Y);
        }
    }
}
