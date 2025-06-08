using Microsoft.Extensions.DependencyInjection;
using ServerEngine.Interfaces.Output;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models.Output;

namespace ServerEngine;

internal abstract class OutputHandler
{
    public abstract OutputMessage CreateMessage();
}

internal class OutputHandler<TCollect, TProvide>
    (
    IOutputCollector<TCollect> writer,
    IEnumerable<IOutputService<TCollect>> services,
    IOutputProvider<TProvide> reader
    ) : OutputHandler
{

    public override OutputMessage CreateMessage()
    {
        foreach (var service in services)
        {
            foreach (var item in service.Pass())
            {
                writer.Pass(item);
            }
        }
        return new OutputMessage<TProvide>(reader);
    }
}

internal class OutputHandler<TProvide>
    (
    IOutputProvider<TProvide> reader
    ) : OutputHandler
{
    public override OutputMessage CreateMessage()
    {
        return new OutputMessage<TProvide>(reader);
    }
}
