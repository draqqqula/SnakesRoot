using ServerEngine.Profiling.Interfaces;
using SessionApi.Services.Profiling.Interfaces;
using System.Text;

namespace SessionApi.Services.Profiling;

public class ConsoleProfileLogger(IProfileAggregator profileAggregator) : IProfileLogger
{
    public void WriteSnapshot()
    {
        using var stream = new MemoryStream();
        profileAggregator.CreateSnapshot(stream);
        stream.Position = 0;
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        var data = reader.ReadToEnd();
        Console.WriteLine(data);
    }
}
