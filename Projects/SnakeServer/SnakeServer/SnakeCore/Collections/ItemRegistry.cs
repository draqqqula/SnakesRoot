using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeCore.Collections;

public class ItemRegistry<TValue, TKey>(IEnumerator<TKey> Keys) : IEnumerable<KeyValuePair<TValue, TKey>>
{
    private readonly Queue<TKey> _released = [];
    private readonly Dictionary<TValue, TKey> _taken = [];

    public IEnumerable<TValue> Values => _taken.Keys;

    public IEnumerator<KeyValuePair<TValue, TKey>> GetEnumerator()
    {
        return _taken.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _taken.GetEnumerator();
    }

    public TKey GetKey(TValue item)
    {
        if (_taken.TryGetValue(item, out var id))
        {
            return id;
        }

        if (_released.TryDequeue(out var released))
        {
            _taken.Add(item, released);
            return released;
        }

        if (!Keys.MoveNext())
        {
            Keys.Reset();
            Keys.MoveNext();
        }
        var key = Keys.Current;
        _taken.Add(item, key);
        return key;
    }

    public bool TryRemove(TValue item)
    {
        if (_taken.TryGetValue(item, out var key))
        {
            _released.Enqueue(key);
            _taken.Remove(item);
            return true;
        }
        return false;
    }
}
