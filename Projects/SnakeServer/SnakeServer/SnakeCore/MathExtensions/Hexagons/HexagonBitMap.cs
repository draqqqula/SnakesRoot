using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeCore.MathExtensions.Hexagons;

public abstract class HexagonBitMap
{
	private readonly BitArray _data;

	public HexagonBitMap(BitArray data)
	{
		_data = data;
	}

	public abstract bool TryGetIndex(int q, int r, out int index);

	public bool? this[int q, int r]
	{
		get
		{
			if (TryGetIndex(q, r, out var index))
			{
                return _data.Get(index);
            }
			return default;
		}
		set 
		{
            if (value is not null && TryGetIndex(q, r, out var index))
            {
				_data.Set(index, value.Value);
            }
		}
	}

	public bool? this[HexagonTile tile]
	{
		get 
		{ 
			return this[tile.Q, tile.R]; 
		}
		set 
		{ 
			this[tile.Q, tile.R] = value; 
		}
	}

    public bool IsTrue(int q, int r)
    {
		var tile = this[q, r];
        return tile.HasValue && tile.Value;
    }

	public bool IsTrue(HexagonTile tile)
	{
		return IsTrue(tile.Q, tile.R);
	}
}
