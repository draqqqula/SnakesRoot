using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerEngine.Interfaces;
using System.Linq.Expressions;

namespace SnakeGame.Configuration;

internal class JsonGameConfiguration : IGameConfiguration
{
    const string Path = "gamesettings.json";

    private readonly JObject _root;

    private readonly Dictionary<string, object> _cache = [];

    public JsonGameConfiguration() 
    {
        using StreamReader file = File.OpenText(Path);
        using JsonTextReader reader = new JsonTextReader(file);
        
        _root = (JObject)JToken.ReadFrom(reader);
    }

    public T? Get<T>(string name)
    {
        {
            if (_cache.TryGetValue(name, out var value))
            {
                return (T)value;
            }
        }
        {
            var value = _root[name];
            if (value is null)
            {
                return default;
            }
            var valid = value.ToObject<T>();
            if (valid is null)
            {
                return default;
            }
            _cache.Add(name, value);
            return valid;
        }
    }
}
