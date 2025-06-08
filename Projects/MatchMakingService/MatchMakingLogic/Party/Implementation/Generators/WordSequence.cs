using System.Collections;
using System.Text;

namespace MatchMakingLogic.Party.Implementation.Generators;

internal class WordSequence : IEnumerable<string>
{
    private readonly char[,] _allowed;
    private int WordLength => _allowed.GetLength(0);
    public WordSequence(char[,] allowed)
    {
        _allowed = allowed;
    }

    public IEnumerator<string> GetEnumerator()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var code in Generate(sb, WordLength))
        {
            yield return code;
        }
    }

    private IEnumerable<string> Generate(StringBuilder sb, int remaining)
    {
        if (remaining < 1)
        {
            yield break;
        }
        for (byte i = 0; i < _allowed.GetLength(1); i++)
        {
            sb.Append(AsChar(i, WordLength - remaining));
            if (remaining == 1)
            {
                yield return sb.ToString();
            }
            else
            {
                foreach (var str in Generate(sb, remaining - 1))
                {
                    yield return str;
                }
            }
            sb.Remove(WordLength - remaining, 1);
        }
    }

    private char AsChar(byte b, int index)
    {
        return _allowed[index, b % _allowed.Length];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
