using SnakeCore.MathExtensions;
using SnakeCore.MathExtensions.Hexagons;
using SnakeGameAssets.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SnakeGameAssets.Services;

internal class MapLoader : IMapLoader
{
    public HexagonBitMap GetMap(string name)
    {
        return BuildMap(ContentLoaderUtility.GetPath($"{name}.bytes"));
    }

    public HexagonBitMap BuildMap(string path)
    {
        using var fs = File.OpenRead(path);
        using var reader = new BinaryReader(fs);

        var startQ = reader.ReadInt32();
        var startR = reader.ReadInt32();
        var endQ = reader.ReadInt32();
        var endR = reader.ReadInt32();
        var length = (endQ - startQ) * (endR - startR);

        var buffer = reader.ReadBytes(Convert.ToInt32(Math.Ceiling(length / 8.0)));

        var data = new BitArray(buffer);

        return new QRCoordinateBasedHexagonMap(data, startQ, startR, endQ, endR);
    }
}
