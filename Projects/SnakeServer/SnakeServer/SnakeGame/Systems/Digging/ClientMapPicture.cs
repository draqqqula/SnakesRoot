using SnakeCore.MathExtensions.Hexagons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

internal class ClientMapPicture
{
    private readonly HashSet<int> _deltaMap;

    public ClientMapPicture()
    {
        _deltaMap = [];
    }

    public ClientMapPicture(IEnumerable<int> tiles)
    {
        _deltaMap = tiles.ToHashSet();
    }

    public void AddTiles(IEnumerable<int> tiles)
    {
        foreach (var tile in tiles)
        {
            _deltaMap.Add(tile);
        }
    }

    public IEnumerable<int> RemoveTiles(IEnumerable<int> tiles)
    {
        foreach (var tile in tiles)
        {
            if (_deltaMap.Contains(tile))
            {
                _deltaMap.Remove(tile);
                yield return tile;
            }
        }
    }
}
