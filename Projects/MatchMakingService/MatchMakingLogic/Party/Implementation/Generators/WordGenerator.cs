using CommunityToolkit.HighPerformance;
using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MatchMakingLogic.Party.Implementation.Generators
{
    internal class WordGenerator : ICodeGenerator
    {
        private const string DefaultCharacters = "123qwe";
        private const int DefaultMinLength = 4;
        private const int DefaultBatchSize = 100;
        private readonly IConfiguration _configuration;

        private char[] _allowedCharacters = [];
        private int _minLength = 0;
        private int _batchSize = 0;

        private IEnumerator<string> Words { get; set; }
        private Queue<string> CodeQueue { get; set; } = new Queue<string>();
        private int Borrowed { get; set; } = 0;

        public WordGenerator(IConfiguration configuration) 
        { 
            _configuration = configuration;
            ReloadConfiguration();
            Words = GenerateInfinite(new Random());
        }

        private void ReloadConfiguration()
        {
            var section = _configuration.GetSection("PartyCodeGenerator");
            _minLength = Convert.ToInt32(section["Length"] ?? DefaultMinLength.ToString());
            var charString = section["Characters"] ?? DefaultCharacters;
            _allowedCharacters = charString.ToCharArray();
            _batchSize = Convert.ToInt32(section["Batch"] ?? DefaultBatchSize.ToString());
        }

        private char[,] GenerateCharMap(Random random, int length)
        {
            var map = new char[length, _allowedCharacters.Length];
            var mapSpan = new Span2D<char>(map);
            for (int i = 0; i < length; i ++) 
            {
                for (int j = 0; j < _allowedCharacters.Length; j++)
                {
                    map[i, j] = _allowedCharacters[j];
                }
                random.Shuffle(mapSpan.GetRowSpan(i));
            }
            return map;
        }

        private IEnumerator<string> GenerateInfinite(Random random)
        {
            var length = _minLength;
            var words = new WordSequence(GenerateCharMap(random, length)).GetEnumerator();
            while (true)
            {
                if (words.MoveNext())
                {
                    yield return words.Current;
                }
                else
                {
                    length += 1;
                    Words = new WordSequence(GenerateCharMap(random, length)).GetEnumerator();
                    Words.MoveNext();
                    yield return Words.Current;
                }
            }
        }

        private IEnumerable<string> GenerateNext(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Words.MoveNext();
                yield return Words.Current;
            }
        }

        public void LoadCodes(Random random, int count)
        {
            var generated = GenerateNext(count).ToArray();
            random.Shuffle(generated);
            foreach (var code in generated)
            {
                CodeQueue.Enqueue(code);
            }
        }

        public PartyCode New()
        {
            if (CodeQueue.Count == 0)
            {
                LoadCodes(new Random(), _batchSize);
            }
            Borrowed += 1;
            return new PartyCode()
            {
                String = CodeQueue.Dequeue()
            };
        }

        public void Free(PartyCode code)
        {
            Borrowed -= 1;
            if (Borrowed == 0)
            {
                Words = GenerateInfinite(new Random());
                CodeQueue.Clear();
            }
        }

        private string AsCodeString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(AsChar(b));
            }
            return sb.ToString();
        }

        private char AsChar(byte b)
        {
            return (char)b;
        }
    }
}
