using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using SnakeGameAssets.Services.Interfaces;
using SnakeGameAssets.Services.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameAssets.Services;

internal class PickupPatternContainer : IPickupPatternContainer, IStartUpService
{
    private Pattern[] _patterns { get; set; } = [];

    public IEnumerable<Pattern> Patterns => _patterns;

    public void Start(IGameContext context)
    {
        _patterns = ExportPatterns(ContentLoaderUtility.GetPath("patterns")).ToArray();
    }

    public IEnumerable<Pattern> ExportPatterns(string path)
    {
        using var fs = File.OpenRead(path);
        var buffer = new byte[8];
        while (fs.Position < fs.Length)
        {
            fs.Read(buffer, 0, 8);
            yield return new Pattern(ToVectors(buffer).ToArray());
        }
    }

    public IEnumerable<Vector2> ToVectors(byte[] map)
    {
        var center = new Vector2(3.5f, 3.5f);
        for (int i = 0; i < 8; i++)
        {
            var b = map[i];
            for (int j = 0; j < 8; j++)
            {
                var bit = (b & (1 << j - 1)) != 0;
                if (bit)
                {
                    yield return new Vector2(i, j) - center;
                }
            }
        }
    }
}
