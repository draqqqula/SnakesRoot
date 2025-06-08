using ServerEngine.Profiling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SessionApi.Services.Profiling;

internal class ProfilingManager : IProfileRecorder, IProfileAggregator
{
    private readonly Dictionary<Type, TimeSpan> _recordings = [];
    private readonly Dictionary<Type, TimeSpan> _actual = [];
    private readonly Dictionary<Type, DateTime> _active = [];

    public void CreateSnapshot(Stream stream)
    {
        using Utf8JsonWriter writer = new Utf8JsonWriter(stream);
        writer.WriteStartObject();
        foreach (var recording in _recordings)
        {
            writer.WriteStartObject(recording.Key.Name);
            writer.WriteNumber("totalExecutionTime_ms", recording.Value.TotalMilliseconds);
            writer.WriteEndObject();
        }
        writer.WriteEndObject();
    }

    public TimeSpan GetTotalExecutionTime(object target)
    {
        return _recordings[target.GetType()];
    }

    public void Start(object target)
    {
        _active.Add(target.GetType(), DateTime.Now);
    }

    public void Stop(object target)
    {
        var type = target.GetType();
        if (_active.TryGetValue(target.GetType(), out var start))
        {
            var duration = DateTime.Now - start;
            if (_recordings.TryGetValue(type, out var total))
            {
                _recordings[type] = total + duration;
            }
            else
            {
                _recordings.Add(type, duration);
            }
            _actual[type] = duration;
            _active.Remove(type);
        }
    }
}
