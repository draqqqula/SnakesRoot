using Newtonsoft.Json.Linq;
using System.Collections;

namespace SnakeCore.Collections;

public class KeyRegistry<T>(IEnumerator<T> Keys) : IEnumerable<T>
{
    private readonly Queue<T> _released = [];
    private HashSet<T> _taken = [];
    public IEnumerator<T> GetEnumerator()
    {
        return _taken.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _taken.GetEnumerator();
    }

    public T GetKey()
    {
        if (_released.TryDequeue(out var key))
        {
            return key;
        }
        if (!Keys.MoveNext())
        {
            Keys.Reset();
            Keys.MoveNext();
        }
        _taken.Add(Keys.Current);
        return Keys.Current;
    }

    public bool TryRemove(T key)
    {
        var removeResult = _taken.Remove(key);
        if (removeResult)
        {
            _released.Enqueue(key);
        }
        return removeResult;
    }
}
